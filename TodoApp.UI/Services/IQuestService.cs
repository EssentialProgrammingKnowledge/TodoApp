using TodoApp.Shared.DTO;

namespace TodoApp.UI.Services
{
    public interface IQuestService
    {
        Task<List<QuestDto>> GetAll();
        Task<QuestDto?> GetById(int id);
        Task Add(QuestDto questDto);
        Task Update(QuestDto questDto);
        Task Delete(int id);
        Task ChangeStatus(ChangeQuestStatus changeQuestStatus);
    }
}
