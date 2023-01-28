using Moq;
using TodoApp.Core.DTO;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;

namespace TodoApp.UnitTests.Services
{
    public class QuestServiceTests
    {
        [Fact]
        public void should_add_quest()
        {
            var dto = new QuestDto { Title = "Title#1" };

            _questService.AddQuest(dto);

            _repository.Verify(r => r.Add(It.IsAny<Quest>()), times: Times.Once);
        }

        public static Quest CreateDefaultQuest(int id = 1, string? title = null, string? description = null, QuestStatus questStatus = QuestStatus.New)
        {
            return new Quest(id, title ?? $"Title#{Guid.NewGuid().ToString("N")}", 
                description ?? "", questStatus, DateTime.UtcNow);
        }

        private readonly IQuestService _questService;
        private readonly Mock<IRepository<Quest>> _repository;

        public QuestServiceTests()
        {
            _repository = new Mock<IRepository<Quest>>();
            _questService = new QuestService(_repository.Object);
        }
    }
}
