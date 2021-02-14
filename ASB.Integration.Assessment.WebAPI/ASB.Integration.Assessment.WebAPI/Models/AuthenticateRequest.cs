using System.ComponentModel.DataAnnotations;

namespace ASB.Integration.Assessment.WebAPI.Models
{
    /// <summary>
    /// Authenticate request.
    /// </summary>
    public class AuthenticateRequest
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
