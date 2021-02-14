using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace ASB.Integration.Assessment.WebAPI.DatabaseContext
{
    /// <summary>
    /// Credit card store Db context Interface.
    /// </summary>
    public interface ICreditCardStoreDbContext
    {
        /// <summary>
        /// Gets or sets the <see cref="DbSet{UserLoginCredentialEntity}"/>.
        /// </summary>
        DbSet<UserLoginCredentialEntity> UserLoginCredentials { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{CreditCardEntity}"/>.
        /// </summary>
        DbSet<CreditCardEntity> CreditCards { get; set; }
    }
}
