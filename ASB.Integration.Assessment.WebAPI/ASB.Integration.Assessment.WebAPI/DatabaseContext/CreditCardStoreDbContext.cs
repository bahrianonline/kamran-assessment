using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ASB.Integration.Assessment.WebAPI.Common;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;

namespace ASB.Integration.Assessment.WebAPI.DatabaseContext
{
    /// <summary>
    /// Credit Card store Db context.
    /// </summary>
    public class CreditCardStoreDbContext : DbContext, ICreditCardStoreDbContext
    {
        private readonly IConfiguration _configuration;

        /// <inheritdoc/>
        public DbSet<UserLoginCredentialEntity> UserLoginCredentials { get; set; }

        /// <inheritdoc/>
        public DbSet<CreditCardEntity> CreditCards { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardStoreDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions"><see cref="DbContextOptions{TContext}"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public CreditCardStoreDbContext(DbContextOptions<CreditCardStoreDbContext> dbContextOptions, IConfiguration configuration)
            : base(dbContextOptions)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLoginCredentialEntity>().HasData(new UserLoginCredentialEntity()
            {
                Id = 1,
                Username = "Asbtestuser1",
                Password = Helper.HashString("123456", _configuration["Secret"]),
                FirstName = "AsbTest",
                LastName = "User",
            });

            modelBuilder.Entity<CreditCardEntity>().HasData(
                new CreditCardEntity()
                {
                    Id = 1,
                    Name = "Test User 1",
                    CardNumber = Helper.Encrypt("4111111111111111"),
                    CardHash = Helper.HashString("4111111111111111", _configuration["Secret"]),
                    CardExpiryDate = new DateTime(2022, 05, 01),
                    Cvc = 123,
                    CreatedAt = DateTime.Now
                },
                new CreditCardEntity()
                {
                    Id = 2,
                    Name = "Test User 2",
                    CardNumber = Helper.Encrypt("5454545454545454"),
                    CardHash = Helper.HashString("5454545454545454", _configuration["Secret"]),
                    CardExpiryDate = new DateTime(2025, 06, 01),
                    Cvc = 321,
                    CreatedAt = DateTime.Now
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
