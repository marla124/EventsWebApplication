using EventsWebApplication.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EventWebApplication.IntegrationTests
{
    public class UserIntegrationTests : BaseIntegrationTest
    {
        private const string BaseUrl = "/api/Auth";

        [Fact]
        public async Task Register_ReturnSuccess()
        {
            var model = new RegisterModel
            {
                Name = "Name",
                Surname = "Surname",
                Email = "testemail3@gmail.com",
                Password = "password12345678",
                PasswordConfirmation = "password12345678"
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var uri = $"{BaseUrl}/Register";
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var returnedUser = JsonConvert.DeserializeObject<UserViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(model.Email, returnedUser!.Email);
        }
    }
}
