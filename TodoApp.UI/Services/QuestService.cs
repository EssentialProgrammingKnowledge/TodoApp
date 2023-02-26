using TodoApp.Shared.DTO;

namespace TodoApp.UI.Services
{
    public class QuestService : IQuestService
    {
        private readonly List<QuestDto> _questDtos = new()
        {
            new QuestDto { Id = 1, Title = "Title#1", Status = "New", Created = DateTime.UtcNow },
            new QuestDto { Id = 2, Title = "Title#2", Status = "InProgress", Created = new DateTime(2023, 2, 26, 10, 20, 30), Description = "Description#2", Modified = new DateTime(2023, 2, 26, 15, 25, 30) },
            new QuestDto { Id = 3, Title = "Title#3", Status = "Complete", Created = new DateTime(2023, 2, 26, 10, 25, 30), Description = "Description#3", Modified = DateTime.Now }
        };

        public void Add(QuestDto questDto)
        {
            questDto.Id = _questDtos.Max(q => q.Id) + 1;
            questDto.Created = DateTime.Now;
            _questDtos.Add(questDto);
        }

        public void ChangeStatus(ChangeQuestStatus changeQuestStatus)
        {
            var quest = _questDtos.SingleOrDefault(q => q.Id == changeQuestStatus.Id);

            if (quest is null)
            {
                return;
            }

            quest.Status = changeQuestStatus.Status;
            quest.Modified = DateTime.Now;
        }

        public void Delete(int id)
        {
            var questToDelete = _questDtos.SingleOrDefault(q => q.Id == id);

            if (questToDelete is null)
            {
                return;
            }

            _questDtos.Remove(questToDelete);
        }

        public List<QuestDto> GetAll()
        {
            return _questDtos;
        }

        public QuestDto? GetById(int id)
        {
            return _questDtos.SingleOrDefault(q => q.Id == id);
        }

        public void Update(QuestDto questDto)
        {
            var questToUpdate = _questDtos.SingleOrDefault(q => q.Id == questDto?.Id);

            if (questToUpdate is null)
            {
                return;
            }

            questToUpdate.Title = questDto.Title;
            questToUpdate.Description = questDto.Description;
            questToUpdate.Status = questDto.Status;
            questToUpdate.Modified = DateTime.Now;
        }
    }
}
