using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using FluentAssertions;
using ASB.Integration.Assessment.WebAPI.Models;

namespace ASB.Integration.Assessment.WebAPI.Test.Controllers
{
    public class AuthenticateControllerTest : IntegrationTest
    {
        public AuthenticateControllerTest(ApiWebApplicationFactory fixture)
            : base(fixture)
        { }

        [Fact]
        public async Task Authenticate_WithUserNameAndPassword_TokenIsNotNull()
        {
            var response = await _client.PostAsync("/api/authenticate/",
                new StringContent("{\"username\":\"Asbtestuser1\",\"password\":\"123456\"}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authenticateResponse = JsonConvert.DeserializeObject<AuthenticateResponse>(
              await response.Content.ReadAsStringAsync()
                );

            Assert.NotNull(authenticateResponse);
            Assert.NotNull(authenticateResponse.Token);
        }

        [Fact]
        public async Task Authenticate_WithInvalidUserNameOrPassword_StatusCodeIsBadRequest()
        {
            var response = await _client.PostAsync("/api/authenticate/",
                new StringContent("{\"username\":\"Asbtestuser2\",\"password\":\"123456\"}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Authenticate_WithoutPassword_StatusCodeIsBadRequest()
        {
            var response = await _client.PostAsync("/api/authenticate/",
                new StringContent("{\"username\":\"Asbtestuser2\"}", Encoding.UTF8, "application/json"));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
