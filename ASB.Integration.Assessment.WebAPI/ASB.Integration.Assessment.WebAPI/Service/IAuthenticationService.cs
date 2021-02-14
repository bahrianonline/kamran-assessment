using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;
using ASB.Integration.Assessment.WebAPI.Models;

namespace ASB.Integration.Assessment.WebAPI.Service
{
    /// <summary>
    /// Authenticate service.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <param name="model"><see cref="AuthenticateRequest"/>.</param>
        /// <returns><see cref="AuthenticateResponse"/>.</returns>
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns><see cref="UserLoginCredentialEntity"/>.</returns>
        UserLoginCredentialEntity GetById(long id);
    }
}
