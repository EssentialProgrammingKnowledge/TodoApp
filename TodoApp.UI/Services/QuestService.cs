using System.Net;
using System.Net.Http.Json;
using TodoApp.Shared.DTO;

namespace TodoApp.UI.Services
{
    public class QuestService : IQuestService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "/api/quests";

        public QuestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task Add(QuestDto questDto)
        {
            return _httpClient.PostAsJsonAsync(BASE_URL, questDto);
        }

        public Task ChangeStatus(ChangeQuestStatus changeQuestStatus)
        {
            return _httpClient.PatchAsJsonAsync($"{BASE_URL}/{changeQuestStatus.Id}", changeQuestStatus);
        }

        public Task Delete(int id)
        {
            return _httpClient.DeleteAsync($"{BASE_URL}/{id}");
        }

        public async Task<List<QuestDto>> GetAll()
        {
            return (await _httpClient.GetFromJsonAsync<List<QuestDto>>(BASE_URL)) ?? new List<QuestDto>();
        }

        public async Task<QuestDto?> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"{BASE_URL}/{id}");

            if (response.StatusCode is HttpStatusCode.NotFound)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<QuestDto>();
        }

        public Task Update(QuestDto questDto)
        {
            return _httpClient.PutAsJsonAsync($"{BASE_URL}/{questDto.Id}", questDto);
        }
    }
}
