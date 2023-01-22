using TodoApp.Core.DTO;

namespace TodoApp.Core.Services
{
    public interface IQuestService : IService<QuestDto>
    {
        QuestDto AddQuest(QuestDto quest);

        QuestDto UpdateQuest(QuestDto quest);

        void DeleteQuest(int id);

        QuestDto? GetQuestById(int id);

        IReadOnlyList<QuestDto> GetAllQuests();
    }
}
