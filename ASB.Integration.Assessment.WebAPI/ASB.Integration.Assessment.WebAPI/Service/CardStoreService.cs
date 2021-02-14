using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ASB.Integration.Assessment.WebAPI.DatabaseContext;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;

namespace ASB.Integration.Assessment.WebAPI.Service
{
    /// <inheritdoc/>
    public class CardStoreService : ICardStoreService
    {
        private readonly CreditCardStoreDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardStoreService"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CardStoreService(CreditCardStoreDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <inheritdoc/>
        public async Task<CreditCardEntity> GetCreditCard(long id)
        {
            return await _context.CreditCards.FirstOrDefaultAsync(record => record.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<CreditCardEntity> GetCreditCard(string cardNumber)
        {
            return await _context.CreditCards.Where(record => record.CardNumber == cardNumber).FirstOrDefaultAsync().ConfigureAwait(false);
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
                creditCardEntity.CardNumber = creditCardEntity.CardNumber;

                _context.CreditCards.Add(creditCardEntity);

                await _context.SaveChangesAsync();

                return creditCardEntity;
            }

            return lookupCreditCard;
        }
    }
}
