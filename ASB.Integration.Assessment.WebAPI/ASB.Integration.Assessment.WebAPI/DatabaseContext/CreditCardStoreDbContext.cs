using System;
using Microsoft.EntityFrameworkCore;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;

namespace ASB.Integration.Assessment.WebAPI.DatabaseContext
{
    /// <summary>
    /// Credit Card store Db context.
    /// </summary>
    public class CreditCardStoreDbContext : DbContext, ICreditCardStoreDbContext
    {
        /// <inheritdoc/>
        public DbSet<UserLoginCredentialEntity> UserLoginCredentials { get; set; }

        /// <inheritdoc/>
        public DbSet<CreditCardEntity> CreditCards { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardStoreDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions"><see cref="DbContextOptions{TContext}"/>.</param>
        public CreditCardStoreDbContext(DbContextOptions<CreditCardStoreDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLoginCredentialEntity>().HasData(new UserLoginCredentialEntity()
            {
                Id = 1,
                Username = "Asbtestuser1",
                Password = "123456",
                FirstName = "AsbTest",
                LastName = "User",
            });

            modelBuilder.Entity<CreditCardEntity>().HasData(
                new CreditCardEntity()
                {
                    Id = 1,
                    Name = "Test User 1",
                    CardNumber = "4111111111111111",
                    CardExpiryDate = new DateTime(2022, 05, 01),
                    Cvc = 123,
                    CreatedAt = DateTime.Now
                },
                new CreditCardEntity()
                {
                    Id = 2,
                    Name = "Test User 2",
                    CardNumber = "5454545454545454",
                    CardExpiryDate = new DateTime(2025, 06, 01),
                    Cvc = 321,
                    CreatedAt = DateTime.Now
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
