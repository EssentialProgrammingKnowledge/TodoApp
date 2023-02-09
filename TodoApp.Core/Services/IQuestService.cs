using TodoApp.Core.DTO;

namespace TodoApp.Core.Services
{
    public interface IQuestService : IService<QuestDto>
    {
        Task<QuestDto> AddQuest(QuestDto quest);

        Task<QuestDto> UpdateQuest(QuestDto quest);

        Task DeleteQuest(int id);

        Task<QuestDto> ChangeQuestStatus(int id, string status);

        Task<QuestDto?> GetQuestById(int id);

        Task<IReadOnlyList<QuestDto>> GetAllQuests();
    }
}
