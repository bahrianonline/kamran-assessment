using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ASB.Integration.Assessment.WebAPI.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace ASB.Integration.Assessment.WebAPI.Test.Controllers
{
    public class CreditCardsControllerTest : IntegrationTest
    {
        public CreditCardsControllerTest(ApiWebApplicationFactory fixture)
            : base(fixture)
        { }

        [Fact]
        public async Task GetAllCreditCards_WithoutAuthorizationToken_StatusCodeIsUnauthorized()
        {
            var response = await _client.GetAsync("/api/creditcards/");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCreditCardById_WithoutAuthorizationToken_StatusCodeIsUnauthorized()
        {
            var response = await _client.GetAsync("/api/creditcards/2");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task PostCreditCard_WithoutAuthorizationToken_StatusCodeIsUnauthorized()
        {
            var response = await _client.PostAsync("/api/creditcards/",
                new StringContent("{\"cardHolderName\":\"Test User 2\",\"cardNumber\":\"5454545454545454\",\"cardExpiryDate\":\"2025-06-01T00:00:00\",\"cvc\":321}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAllCreditCards_WithAuthorizationToken_AllCardsReceived()
        {
            var response = await _client.PostAsync("/api/authenticate/",
                    new StringContent("{\"username\":\"Asbtestuser1\",\"password\":\"123456\"}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authenticateResponse = JsonConvert.DeserializeObject<AuthenticateResponse>(
              await response.Content.ReadAsStringAsync()
                );

            Assert.NotNull(authenticateResponse);
            Assert.NotNull(authenticateResponse.Token);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authenticateResponse.Token);
            var responseOfGetAllCreditCards = await _client.GetAsync("/api/creditcards/");
            responseOfGetAllCreditCards.StatusCode.Should().Be(HttpStatusCode.OK);

            var listOfCreditCards = JsonConvert.DeserializeObject<CreditCardModel[]>(
                await responseOfGetAllCreditCards.Content.ReadAsStringAsync()
            );

            Assert.NotNull(listOfCreditCards);
            Assert.NotEmpty(listOfCreditCards);
        }

        [Fact]
        public async Task GetCreditCardById1_WithAuthorizationToken_CardsReceived()
        {
            var response = await _client.PostAsync("/api/authenticate/",
                    new StringContent("{\"username\":\"Asbtestuser1\",\"password\":\"123456\"}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authenticateResponse = JsonConvert.DeserializeObject<AuthenticateResponse>(
              await response.Content.ReadAsStringAsync()
                );

            Assert.NotNull(authenticateResponse);
            Assert.NotNull(authenticateResponse.Token);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authenticateResponse.Token);
            var responseOfGetCreditCardById = await _client.GetAsync("/api/creditcards/1");
            responseOfGetCreditCardById.StatusCode.Should().Be(HttpStatusCode.OK);

            var creditCard = JsonConvert.DeserializeObject<CreditCardModel>(
                await responseOfGetCreditCardById.Content.ReadAsStringAsync()
            );

            Assert.NotNull(creditCard);
            Assert.Equal(1, creditCard.CardStoreId);
        }

        [Fact]
        public async Task PostExistingCardNumberToAddRecord_WithAuthorizationToken_ReturnsCardDetails()
        {
            var response = await _client.PostAsync("/api/authenticate/",
                    new StringContent("{\"username\":\"Asbtestuser1\",\"password\":\"123456\"}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authenticateResponse = JsonConvert.DeserializeObject<AuthenticateResponse>(
              await response.Content.ReadAsStringAsync()
                );

            Assert.NotNull(authenticateResponse);
            Assert.NotNull(authenticateResponse.Token);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authenticateResponse.Token);
            var postCreditCardDetailToAdd = await _client.PostAsync("/api/creditcards/", 
                new StringContent("{\"cardHolderName\":\"Test User 2\",\"cardNumber\":\"5454545454545454\",\"cardExpiryDate\":\"2025-06-01T00:00:00\",\"cvc\":321}", Encoding.UTF8, "application/json"));
            postCreditCardDetailToAdd.StatusCode.Should().Be(HttpStatusCode.Created);

            var creditCard = JsonConvert.DeserializeObject<CreditCardModel>(
                await postCreditCardDetailToAdd.Content.ReadAsStringAsync()
            );

            Assert.NotNull(creditCard);
            Assert.Equal("5454545454545454", creditCard.CardNumber);
        }

        [Fact]
        public async Task PostNewCardNumberToAddRecord_WithAuthorizationToken_ReturnsCardDetails()
        {
            var response = await _client.PostAsync("/api/authenticate/",
                    new StringContent("{\"username\":\"Asbtestuser1\",\"password\":\"123456\"}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authenticateResponse = JsonConvert.DeserializeObject<AuthenticateResponse>(
              await response.Content.ReadAsStringAsync()
                );

            Assert.NotNull(authenticateResponse);
            Assert.NotNull(authenticateResponse.Token);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authenticateResponse.Token);
            var postCreditCardDetailToAdd = await _client.PostAsync("/api/creditcards/",
                new StringContent("{\"cardHolderName\":\"Test User 2\",\"cardNumber\":\"5454545454545452\",\"cardExpiryDate\":\"2025-06-01T00:00:00\",\"cvc\":321}", Encoding.UTF8, "application/json"));
            postCreditCardDetailToAdd.StatusCode.Should().Be(HttpStatusCode.Created);

            var creditCard = JsonConvert.DeserializeObject<CreditCardModel>(
                await postCreditCardDetailToAdd.Content.ReadAsStringAsync()
            );

            Assert.NotNull(creditCard);
            Assert.Equal("5454545454545452", creditCard.CardNumber);
        }
    }
}
