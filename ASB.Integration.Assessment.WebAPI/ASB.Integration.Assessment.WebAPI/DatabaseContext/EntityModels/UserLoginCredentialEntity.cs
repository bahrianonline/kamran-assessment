using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels
{
    /// <summary>
    /// User login credential.
    /// </summary>
    [Table("tblUserLoginCredential")]
    public class UserLoginCredentialEntity
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
    }
}
