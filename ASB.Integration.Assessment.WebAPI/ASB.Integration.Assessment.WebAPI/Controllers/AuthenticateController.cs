using System;
using Microsoft.AspNetCore.Mvc;
using ASB.Integration.Assessment.WebAPI.Service;
using ASB.Integration.Assessment.WebAPI.Models;

namespace ASB.Integration.Assessment.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateController"/> class.
        /// </summary>
        /// <param name="authenticationService"><see cref="IAuthenticationService"/>.</param>
        public AuthenticateController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _authenticationService.Authenticate(model);

            if (response == null)
            {
                return BadRequest(new { message = "UserName or password is incorrect" });
            }

            return Ok(response);
        }
    }
}
