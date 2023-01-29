using Shouldly;
using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.UnitTests.Common;

namespace TodoApp.UnitTests.Entities
{
    public class QuestTests
    {
        [Fact]
        public void should_create_quest()
        {
            // Arrange  inaczej given
            var title = "Quest#1";
            var description = "";
            var beforeCreated = DateTime.UtcNow;

            // Act  	inacej when
            var quest = Quest.Create(title, description);

            // Assert	inaczej then
            var afterCreated = DateTime.UtcNow;
            quest.ShouldNotBeNull();
            quest.Title.ShouldBe(title);
            quest.Description.ShouldBe(description);
            quest.Status.ShouldBe(QuestStatus.New);
            quest.Created.ShouldBeGreaterThan(beforeCreated);
            quest.Created.ShouldBeLessThan(afterCreated);
            quest.Modified.ShouldBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("         ")]
        [InlineData("a")]
        public void given_invalid_title_when_create_quest_should_throw_an_exception(string title)
        {
            // Arrange  inaczej given
            var description = "";

            // Act  	inacej when
            var exception = Record.Exception(() => Quest.Create(title, description));

            // Assert	inaczej then
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CustomException>();
            exception.Message.ShouldContain("Title");
        }

        [Fact]
        public void given_valid_quest_should_change_title()
        {
            var quest = Fixture.CreateDefaultQuest();
            var title = "New Title";

            quest.ChangeTitle(title);

            quest.Title.ShouldBe(title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData("n")]
        public void given_invalid_title_when_change_quest_title_should_throw_an_exception(string title)
        {
            var quest = Fixture.CreateDefaultQuest();

            var exception =  Record.Exception(() => quest.ChangeTitle(title));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CustomException>();
            exception.Message.ShouldContain("Title");
        }

        [Fact]
        public void given_valid_quest_should_change_status()
        {
            var quest = Fixture.CreateDefaultQuest();
            var status = QuestStatus.InProgress;

            quest.ChangeStatus(status.ToString());

            quest.Status.ShouldBe(status);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData("200")]
        public void given_invalid_quest_status_when_change_quest_status_should_throw_an_exception(string status)
        {
            var quest = Fixture.CreateDefaultQuest();
            var expectedException = new CustomException($"There is no Quest status {status}");

            var exception = Record.Exception(() => quest.ChangeStatus(status));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Theory]
        [MemberData(nameof(TestDateData))]
        public void should_create_quest_with_modified_date(DateTime? modifiedDate)
        {
            var quest = new Quest(10, "Title", "Description", "Complete", DateTime.UtcNow, modifiedDate);

            quest.ShouldNotBeNull();
            quest.Title.ShouldNotBeNullOrWhiteSpace();
            quest.Description.ShouldNotBeNullOrWhiteSpace();
            quest.Created.ShouldBeGreaterThan(default);
            quest.Modified.ShouldBe(modifiedDate);
        }

        public static IEnumerable<object?[]> TestDateData()
        {
            yield return new object?[] { DateTime.UtcNow };
            yield return new object?[] { null };
        }
    }
}