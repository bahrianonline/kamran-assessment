using System.Collections.Generic;
using System.Threading.Tasks;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;

namespace ASB.Integration.Assessment.WebAPI.Service
{
    /// <summary>
    /// Card store service.
    /// </summary>
    public interface ICardStoreService
    {
        /// <summary>
        /// Get credit cards.
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> where TResult is <see cref="IEnumerable{CreditCardEntity}"/>.</returns>
        Task<IEnumerable<CreditCardEntity>> GetCreditCards();

        /// <summary>
        /// Get credit card.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Task{CreditCardEntity}"/>.</returns>
        Task<CreditCardEntity> GetCreditCard(long id);

        /// <summary>
        /// Insert.
        /// </summary>
        /// <param name="creditCard"><see cref="CreditCardEntity"/>.</param>
        /// <returns><see cref="Task{CreditCardEntity}"/>.</returns>
        Task<CreditCardEntity> Insert(CreditCardEntity creditCard);
    }
}
