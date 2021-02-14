using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using ASB.Integration.Assessment.WebAPI.Service;

namespace ASB.Integration.Assessment.WebAPI.Common
{
    /// <summary>
    /// JWT Middle ware.
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Invoke authorization.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <param name="authenticationService"><see cref="IAuthenticationService"/>.</param>
        /// <returns>Context.</returns>
        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachUserToContext(context, authenticationService, token);
            }

            await _next(context);
        }

        /// <summary>
        /// Attach user to context.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <param name="authenticationService"><see cref="IAuthenticationService"/>.</param>
        /// <param name="token">Token.</param>
        private void AttachUserToContext(HttpContext context, IAuthenticationService authenticationService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                context.Items["User"] = authenticationService.GetById(userId);
            }
            catch
            {
                // Do nothing if JWT validation fails
                // User is not attached to context so request won't have access to secure routes
            }
        }
    }
}
