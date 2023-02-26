using TodoApp.Shared.DTO;

namespace TodoApp.UI.Services
{
    public interface IQuestService
    {
        List<QuestDto> GetAll();
        QuestDto? GetById(int id);
        void Add(QuestDto questDto);
        void Update(QuestDto questDto);
        void Delete(int id);
        void ChangeStatus(ChangeQuestStatus changeQuestStatus);
    }
}
