using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels
{
    /// <summary>
    /// Credit Card Entity.
    /// </summary>
    [Table("tblCreditCard")]
    public class CreditCardEntity
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card hash.
        /// </summary>
        public string CardHash { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the card expiry date.
        /// </summary>
        public DateTime CardExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the cvc.
        /// </summary>
        public int Cvc { get; set; }

        /// <summary>
        /// Gets or sets the date time when it was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
