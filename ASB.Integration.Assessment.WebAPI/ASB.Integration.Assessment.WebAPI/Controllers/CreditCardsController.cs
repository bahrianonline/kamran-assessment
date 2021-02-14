using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASB.Integration.Assessment.WebAPI.Service;
using ASB.Integration.Assessment.WebAPI.Models;

namespace ASB.Integration.Assessment.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        private ICardStoreService _cardStoreService;
        private readonly IMapper _mapper;

        public CreditCardsController(ICardStoreService cardStoreService, IMapper mapper)
        {
            _cardStoreService = cardStoreService ?? throw new ArgumentNullException(nameof(cardStoreService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/CreditCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreditCardModel>>> GetCreditCards()
        {
            var result = await _cardStoreService.GetCreditCards();
            return Ok(_mapper.Map<List<CreditCardModel>>(result));
        }

        // GET: api/CreditCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CreditCardModel>> GetCreditCard(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"{nameof(id)} cannot be zero or less than zero");
            }

            var creditCardEntity = await _cardStoreService.GetCreditCard(id);

            if (creditCardEntity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CreditCardModel>(creditCardEntity));
        }

        // POST: api/CreditCards
        [HttpPost]
        public async Task<ActionResult<CreditCardModel>> PostCreditCard(CreditCardModel creditCard)
        {
            if (creditCard is null)
            {
                throw new ArgumentNullException(nameof(creditCard));
            }

            if (creditCard.CardStoreId.HasValue)
            {
                throw new InvalidOperationException($"{nameof(Models.CreditCardModel.CardStoreId)} cannot be supplied");
            }

            var result = await _cardStoreService.Insert(_mapper.Map<DatabaseContext.EntityModels.CreditCardEntity>(creditCard));

            creditCard.CardStoreId = result.Id;

            return CreatedAtAction("GetCreditCard", new { id = result.Id }, creditCard);
        }
    }
}
