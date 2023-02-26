using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TodoApp.Shared.DTO;
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

        [Fact]
        public async Task given_invalid_quest_when_add_should_return_status_code_400()
        {
            var quest = new QuestDto { Title = "", Description = "" };

            var response = await Client.PostAsJsonAsync(PATH, quest);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var validationMessage = await response.Content.ReadFromJsonAsync<ErrorModel>();
            validationMessage.Errors.ShouldNotBeNull();
            validationMessage.Errors.ShouldNotBeEmpty();
            validationMessage.Errors.ShouldContain(v => v.Key == "Title");
        }

        [Fact]
        public async Task should_update_quest_and_return_status_204()
        {
            var quest = await AddDefaultQuest();
            quest.Title = "Title#3";
            quest.Description = "Description#3";
            quest.Status = "InProgress";

            var response = await Client.PutAsJsonAsync($"{PATH}/{quest.Id}", quest);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_quest()
        {
            var quest = await AddDefaultQuest();
            quest.Title = "Title#4";
            quest.Description = "Description#4";
            quest.Status = "Complete";

            await Client.PutAsJsonAsync($"{PATH}/{quest.Id}", quest);

            var questUpdated = await Client.GetFromJsonAsync<QuestDto>($"{PATH}/{quest.Id}");
            questUpdated.Title.ShouldBe(quest.Title);
            questUpdated.Description.ShouldBe(quest.Description);
            questUpdated.Status.ShouldBe(quest.Status);
            questUpdated.Modified.ShouldNotBeNull();
        }

        [Fact]
        public async Task given_invalid_quest_when_update_should_return_status_code_400()
        {
            var quest = await AddDefaultQuest();
            quest.Title = "";
            quest.Description = "";

            var response = await Client.PutAsJsonAsync($"{PATH}/{quest.Id}", quest);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var validationMessage = await response.Content.ReadFromJsonAsync<ErrorModel>();
            validationMessage.Errors.ShouldNotBeNull();
            validationMessage.Errors.ShouldNotBeEmpty();
            validationMessage.Errors.ShouldContain(v => v.Key == "Title");
        }

        [Fact]
        public async Task should_change_quest_status_and_return_status_204()
        {
            var quest = await AddDefaultQuest();
            var questStatus = new ChangeQuestStatus(quest.Id, "InProgress");

            var response = await Client.PatchAsJsonAsync($"{PATH}/{quest.Id}", questStatus);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_change_quest_status()
        {
            var quest = await AddDefaultQuest();
            var questStatus = new ChangeQuestStatus(quest.Id, "Complete");

            await Client.PatchAsJsonAsync($"{PATH}/{quest.Id}", questStatus);

            var questUpdated = await Client.GetFromJsonAsync<QuestDto>($"{PATH}/{quest.Id}");
            questUpdated.Status.ShouldBe(questStatus.Status);
        }

        [Fact]
        public async Task given_invalid_status_when_change_quest_status_should_return_400()
        {
            var quest = await AddDefaultQuest();
            var questStatus = new ChangeQuestStatus(quest.Id, "");

            var response = await Client.PatchAsJsonAsync($"{PATH}/{quest.Id}", questStatus);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var validationMessage = await response.Content.ReadFromJsonAsync<ErrorModel>();
            validationMessage.Errors.ShouldNotBeNull();
            validationMessage.Errors.ShouldNotBeEmpty();
            validationMessage.Errors.ShouldContain(v => v.Key == "Status");
        }

        [Fact]
        public async Task should_delete_quest_and_return_status_code_204()
        {
            var quest = await AddDefaultQuest();

            var response = await Client.DeleteAsync($"{PATH}/{quest.Id}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_delete_quest()
        {
            var quest = await AddDefaultQuest();

            await Client.DeleteAsync($"{PATH}/{quest.Id}");

            var response = await Client.GetAsync($"{PATH}/{quest.Id}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task given_invalid_quest_id_when_delete_quest_should_return_400()
        {
            var id = 2000;

            var response = await Client.DeleteAsync($"{PATH}/{id}");

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            errors.ShouldContain(e => e.Value.Contains($"Quest with id: '{id}' was not found"));
        }

        [Fact]
        public async Task should_get_all_quests()
        {
            var quest1 = await AddDefaultQuest();
            var quest2 = await AddDefaultQuest();

            var response = await Client.GetAsync(PATH);
            var quests = await response.Content.ReadFromJsonAsync<List<QuestDto>>();

            quests.ShouldNotBeNull();
            quests.ShouldNotBeEmpty();
            quests.Count.ShouldBeGreaterThan(1);
            quests.ShouldContain(q => q.Id == quest1.Id);
            quests.ShouldContain(q => q.Id == quest2.Id);
        }

        private async Task<QuestDto> AddDefaultQuest()
        {
            var quest = new QuestDto { Title = $"Title#{Guid.NewGuid():N}", Description = "Description#2" };
            var response = await Client.PostAsJsonAsync(PATH, quest);
            var id = GetIdFromHeader(response.Headers);
            quest.Id = id;
            return quest;
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
