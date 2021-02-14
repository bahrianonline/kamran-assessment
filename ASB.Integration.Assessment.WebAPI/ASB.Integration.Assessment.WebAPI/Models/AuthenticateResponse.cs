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
