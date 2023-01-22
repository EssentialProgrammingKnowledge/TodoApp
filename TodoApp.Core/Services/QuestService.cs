using TodoApp.Core.DTO;
using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Mappings;
using TodoApp.Core.Repositories;

namespace TodoApp.Core.Services
{
    internal sealed class QuestService : IQuestService
    {
        private readonly IRepository<Quest> _repository;

        public QuestService(IRepository<Quest> repository)
        {
            _repository = repository;
        }

        public QuestDto AddQuest(QuestDto dto)
        {
            var quest = Quest.Create(dto.Title, dto.Description);
            _repository.Add(quest);
            return quest.AsDto();
        }

        public QuestDto UpdateQuest(QuestDto dto)
        {
            var quest = _repository.Get(dto.Id);

            if (quest is null)
            {
                throw new CustomException($"Quest with id {dto.Id} was not found");
            }

            quest.ChangeTitle(dto.Title);
            quest.ChangeDescription(dto.Description);
            quest.ChangeStatus(dto.Status);
            return quest.AsDto();
        }

        public void DeleteQuest(int id)
        {
            var quest = _repository.Get(id);

            if (quest is null)
            {
                throw new CustomException($"Quest with id {id} was not found");
            }

            _repository.Delete(quest);
        }

        public QuestDto? GetQuestById(int id)
        {
            return _repository.Get(id)?.AsDto();
        }

        public IReadOnlyList<QuestDto> GetAllQuests()
        {
            var quests = _repository.GetAll();
            var dtos = new List<QuestDto>();

            foreach (var quest in quests)
            {
                dtos.Add(quest.AsDto());
            }

            return dtos;
        }
    }
}
