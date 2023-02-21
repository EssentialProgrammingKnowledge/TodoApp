using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TodoApp.Core.DTO;
using TodoApp.IntegrationTests.Common;

namespace TodoApp.IntegrationTests.Controllers
{
    public class QuestsControllerTests : BaseTest
    {
        [Fact]
        public async Task should_add_quest_and_return_status_code_201()
        {
            var quest = new QuestDto { Title = "Title#1", Description = "Description#1" };

            var response = await Client.PostAsJsonAsync(PATH, quest);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task should_add_quest_to_db()
        {
            var quest = new QuestDto { Title = "Title#2", Description = "Description#2" };

            var response = await Client.PostAsJsonAsync(PATH, quest);

            var id = GetIdFromHeader(response.Headers);
            var questAdded = await Client.GetFromJsonAsync<QuestDto>($"{PATH}/{id}");
            questAdded.ShouldNotBeNull();
            questAdded.Title.ShouldBe(quest.Title);
            questAdded.Description.ShouldBe(quest.Description);
        }

        private int GetIdFromHeader(HttpResponseHeaders httpResponseHeader)
        {
            var (responseHeaderName, responseHeaderValue) = httpResponseHeader.Where(h => h.Key == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.First().Split(PATH + '/');
            int.TryParse(splitted[1], out var id);
            id.ShouldNotBe(default);
            return id;
        }

        private const string PATH = "api/Quests";

        public QuestsControllerTests(TestApplicationFactory testApplicationFactory) : base(testApplicationFactory)
        {
        }
    }
}
