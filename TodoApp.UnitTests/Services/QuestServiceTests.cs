using Moq;
using Shouldly;
using TodoApp.Core.DTO;
using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;
using TodoApp.UnitTests.Common;

namespace TodoApp.UnitTests.Services
{
    public class QuestServiceTests
    {
        [Fact]
        public async Task should_add_quest()
        {
            var dto = new QuestDto { Title = "Title#1" };

            await _questService.AddQuest(dto);

            _repository.Verify(r => r.Add(It.IsAny<Quest>()), times: Times.Once);
        }

        [Fact]
        public async Task should_quest_status()
        {
            var quest = Fixture.CreateDefaultQuest();
            _repository.Setup(r => r.Get(quest.Id)).ReturnsAsync(quest);
            var status = QuestStatus.Complete.ToString();

            var questAfterUpdate = await _questService.ChangeQuestStatus(quest.Id, status);

            questAfterUpdate.ShouldNotBeNull();
            _repository.Verify(r => r.Update(quest));
            questAfterUpdate.Status.ShouldBe(status);
        }

        [Fact]
        public async Task given_invalid_id_when_change_quest_status_should_throw_an_exception()
        {
            var id = 1;
            var status = "avbc";

            var exception = await Record.ExceptionAsync(() => _questService.ChangeQuestStatus(id, status));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CustomException>();
            exception.Message.ShouldContain("not found");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData("nie+istnieje")]
        [InlineData("200")]
        public async Task given_invalid_status_when_change_quest_status_should_throw_an_exception(string status)
        {
            var quest = Fixture.CreateDefaultQuest();
            _repository.Setup(r => r.Get(quest.Id)).ReturnsAsync(quest);
            var expectedException = new CustomException($"There is no Quest status {status}");

            var exception = await Record.ExceptionAsync(() => _questService.ChangeQuestStatus(quest.Id, status));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task should_update_quest()
        {
            var quest = Fixture.CreateDefaultQuest();
            _repository.Setup(r => r.Get(quest.Id)).ReturnsAsync(quest);
            var dto = new QuestDto { Id = quest.Id, Title = "New Title", Description = "Desc123", Status = QuestStatus.InProgress.ToString() };

            var dtoAfterUpdate = await _questService.UpdateQuest(dto);

            _repository.Verify(r => r.Update(quest), times: Times.Once);
            dtoAfterUpdate.ShouldNotBeNull();
            dtoAfterUpdate.Title.ShouldBe(dto.Title);
            dtoAfterUpdate.Description.ShouldBe(dto.Description);
            dtoAfterUpdate.Status.ShouldBe(dto.Status);
        }

        [Fact]
        public async Task given_invalid_id_when_update_quest_should_throw_an_exception()
        {
            var id = 1;
            var dto = new QuestDto { Id = id };

            var exception = await Record.ExceptionAsync(() => _questService.UpdateQuest(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CustomException>();
            exception.Message.ShouldContain("not found");
            _repository.Verify(r => r.Update(It.IsAny<Quest>()), times: Times.Never);
        }

        [Fact]
        public async Task should_delete_quest()
        {
            var quest = Fixture.CreateDefaultQuest();
            _repository.Setup(r => r.Get(quest.Id)).ReturnsAsync(quest);

            _questService.DeleteQuest(quest.Id);

            _repository.Verify(r => r.Delete(quest), times: Times.Once);
        }

        [Fact]
        public async Task given_invalid_id_when_delete_quest_should_throw_an_exception()
        {
            var id = 1;

            var exception = await Record.ExceptionAsync(() => _questService.DeleteQuest(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CustomException>();
            exception.Message.ShouldContain("not found");
            _repository.Verify(r => r.Delete(It.IsAny<Quest>()), times: Times.Never);
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
