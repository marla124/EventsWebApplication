using EventsWebApplication.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EventWebApplication.IntegrationTests
{
    public class EventControllerIntegrationTest : BaseIntegrationTest
    {
        private const string BaseUrl = "/api/Event";

        [Fact]
        public async Task Create_ReturnSuccess()
        {
            var category = await PopulateCategoryToDatabase();

            var model = new EventModel()
            {
                Name = "Name",
                DateAndTime = DateTime.UtcNow,
                Address = "address",
                Description = "string",
                CategoryId = category.Id,
                MaxNumberOfPeople = 10,
                Image = null
            };
            var uri = $"{BaseUrl}/CreateEvent";
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedProject = JsonConvert.DeserializeObject<EventModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(model.Name, returnedProject!.Name);
            Assert.Equal(model.Description, returnedProject.Description);
        }
    }
}
