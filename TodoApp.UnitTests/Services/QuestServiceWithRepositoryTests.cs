using Shouldly;
using TodoApp.Shared.DTO;
using TodoApp.Core.Services;
using TodoApp.Domain.Entities;
using TodoApp.UnitTests.Common;

namespace TodoApp.UnitTests.Services
{
    public class QuestServiceWithRepositoryTests
    {
        [Fact]
        public async Task should_add_quest()
        {
            var quest = new QuestDto { Title = "Title#1", Description = "Description#1" };

            var questAdded = await _questService.AddQuest(quest);

            questAdded.ShouldNotBeNull();
            var questFromRepo = await _questRepository.Get(questAdded.Id);
            questFromRepo.ShouldNotBeNull();
            questFromRepo.Title.ShouldBe(quest.Title);
            questFromRepo.Description.ShouldBe(quest.Description);
            questFromRepo.Status.ToString().ShouldBe(questAdded.Status);
        }

        private readonly IQuestService _questService;
        private readonly InMemoryRepository<Quest> _questRepository;

        public QuestServiceWithRepositoryTests()
        {
            _questRepository = new InMemoryRepository<Quest>();
            _questService = new QuestService(_questRepository);
        }
    }
}
