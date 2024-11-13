using EventsWebApplication.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EventWebApplication.IntegrationTests
{
    public class AuthIntegrationTests : BaseIntegrationTest
    {
        private const string BaseUrl = "/api/Auth";

        [Fact]
        public async Task Login_ReturnSuccess()
        {
            var user = await PopulateUserToDatabase();
            var model = new LoginModel
            {
                Email = user.Email,
                Password = "password"
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var uri = $"{BaseUrl}/Login";
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Refresh_ReturnSuccess()
        {
            var token = await PopulateTokenToDatabase();
            var model = new RefreshTokenModel
            {
                RefreshToken = token.Id.ToString(),
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var uri = $"{BaseUrl}/Refresh";
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteById_ReturnSuccess()
        {
            var token = await PopulateTokenToDatabase();
            var model = new RefreshTokenModel()
            {
                RefreshToken = token.Id.ToString()
            };
            var uri = $"{BaseUrl}/Revoke/{model.RefreshToken}";
            var response = await _httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}