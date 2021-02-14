using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ASB.Integration.Assessment.WebAPI.DatabaseContext;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;
using ASB.Integration.Assessment.WebAPI.Common;

namespace ASB.Integration.Assessment.WebAPI.Service
{
    /// <inheritdoc/>
    public class CardStoreService : ICardStoreService
    {
        private readonly CreditCardStoreDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardStoreService"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public CardStoreService(CreditCardStoreDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        /// <inheritdoc/>
        public async Task<CreditCardEntity> GetCreditCard(long id)
        {
            return await _context.CreditCards.FirstOrDefaultAsync(record => record.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<CreditCardEntity> GetCreditCard(string cardNumber)
        {
            return await _context.CreditCards.Where(record => record.CardHash == Helper.HashString(cardNumber, _configuration["Secret"])).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CreditCardEntity>> GetCreditCards()
        {
            return await _context.CreditCards.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<CreditCardEntity> Insert(CreditCardEntity creditCardEntity)
        {
            if (creditCardEntity is null)
            {
                throw new ArgumentNullException(nameof(creditCardEntity));
            }

            var lookupCreditCard = await GetCreditCard(creditCardEntity.CardNumber);

            if (lookupCreditCard == null)
            {
                creditCardEntity.CardHash = Helper.HashString(creditCardEntity.CardNumber, _configuration["Secret"]);
                creditCardEntity.CardNumber = Helper.Encrypt(creditCardEntity.CardNumber);

                _context.CreditCards.Add(creditCardEntity);

                await _context.SaveChangesAsync();

                return creditCardEntity;
            }

            return lookupCreditCard;
        }
    }
}
