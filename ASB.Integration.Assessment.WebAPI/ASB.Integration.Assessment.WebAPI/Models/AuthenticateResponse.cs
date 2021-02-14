using System;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;

namespace ASB.Integration.Assessment.WebAPI.Models
{
    /// <summary>
    /// Authenticate response.
    /// </summary>
    public class AuthenticateResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateResponse"/> class.
        /// </summary>
        /// <param name="user"><see cref="UserLoginCredentialEntity"/>.</param>
        /// <param name="token">Token.</param>
        public AuthenticateResponse(UserLoginCredentialEntity user, string token)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException($"'{nameof(token)}' cannot be null or whitespace", nameof(token));
            }

            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Token = token;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateResponse"/> class.
        /// </summary>
        public AuthenticateResponse()
        { }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets the token.
        /// </summary>
        public string Token { get; set; }
    }
}
