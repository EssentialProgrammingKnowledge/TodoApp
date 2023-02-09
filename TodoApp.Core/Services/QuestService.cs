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

        public async Task<QuestDto> AddQuest(QuestDto dto)
        {
            var quest = Quest.Create(dto.Title, dto.Description);
            await _repository.Add(quest);
            return quest.AsDto();
        }

        public async Task<QuestDto> UpdateQuest(QuestDto dto)
        {
            var quest = await _repository.Get(dto.Id);

            if (quest is null)
            {
                throw new CustomException($"Quest with id {dto.Id} was not found");
            }

            quest.ChangeTitle(dto.Title);
            quest.ChangeDescription(dto.Description);
            quest.ChangeStatus(dto.Status);
            await _repository.Update(quest);
            return quest.AsDto();
        }

        public async Task DeleteQuest(int id)
        {
            var quest = await _repository.Get(id);

            if (quest is null)
            {
                throw new CustomException($"Quest with id {id} was not found");
            }

            await _repository.Delete(quest);
        }

        public async Task<QuestDto?> GetQuestById(int id)
        {
            return (await _repository.Get(id))?.AsDto();
        }

        public async Task<IReadOnlyList<QuestDto>> GetAllQuests()
        {
            var quests = await _repository.GetAll();
            var dtos = new List<QuestDto>();

            foreach (var quest in quests)
            {
                dtos.Add(quest.AsDto());
            }

            return dtos;
        }

        public async Task<QuestDto> ChangeQuestStatus(int id, string status)
        {
            var quest = await _repository.Get(id);

            if (quest is null)
            {
                throw new CustomException($"Quest with id: '{id}' was not found");
            }

            quest.ChangeStatus(status);
            await _repository.Update(quest);
            return quest.AsDto();
        }
    }
}
