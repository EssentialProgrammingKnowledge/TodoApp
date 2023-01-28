using Shouldly;
using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;

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
    }
}