using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ASB.Integration.Assessment.WebAPI.DatabaseContext;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;
using ASB.Integration.Assessment.WebAPI.Models;
using Microsoft.Extensions.Configuration;

namespace ASB.Integration.Assessment.WebAPI.Service
{
    /// <inheritdoc/>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly CreditCardStoreDbContext _context;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="context"><see cref="ICreditCardStoreDbContext"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public AuthenticationService(CreditCardStoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var loggedInUser = _context.UserLoginCredentials.Where(
                user => user.Username == model.Username &&
                user.Password == model.Password).FirstOrDefault();

            if (loggedInUser == null)
            {
                return null;
            }

            return new AuthenticateResponse(loggedInUser, GenerateJwtToken(loggedInUser));
        }


        /// <inheritdoc/>
        public UserLoginCredentialEntity GetById(long id)
        {
            return _context.UserLoginCredentials.Find(id);
        }

        /// <summary>
        /// Generate JWT token.
        /// </summary>
        /// <param name="loggedInUser"><see cref="UserLoginCredentialEntity"/>.</param>
        /// <returns>Token.</returns>
        private string GenerateJwtToken(UserLoginCredentialEntity loggedInUser)
        {
            // Generate token that is valid for 7 days.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", loggedInUser.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
