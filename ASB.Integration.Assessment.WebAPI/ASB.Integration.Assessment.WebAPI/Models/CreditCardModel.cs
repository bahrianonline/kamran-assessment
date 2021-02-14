using System;
using System.ComponentModel.DataAnnotations;

namespace ASB.Integration.Assessment.WebAPI.Models
{
    /// <summary>
    /// Credit card model.
    /// </summary>
    public class CreditCardModel
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public long? CardStoreId { get; set; }

        /// <summary>
        /// Gets or sets the card holder name.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string CardHolderName { get; set; }

        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        [Required]
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card expiry date.
        /// </summary>
        [Required]
        public DateTime CardExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the cvc.
        /// </summary>
        [Required]
        public int Cvc { get; set; }

        /// <summary>
        /// Gets or sets the date time when it was posted.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
