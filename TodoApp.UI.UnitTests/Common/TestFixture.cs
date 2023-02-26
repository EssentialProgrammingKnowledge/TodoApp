using TodoApp.Shared.DTO;

namespace TodoApp.UI.UnitTests.Common
{
    internal class TestFixture
    {
        public static IEnumerable<QuestDto> GetQuests()
        {
            yield return new QuestDto { Id = 1, Title = "Quest#1", Status = "New", Created = DateTime.UtcNow };
            yield return new QuestDto { Id = 2, Title = "Quest#2", Status = "InProgress", Created = new DateTime(2023, 2, 26, 10, 20, 30), Description = "Description#2", Modified = new DateTime(2023, 2, 26, 15, 25, 30) };
            yield return new QuestDto { Id = 3, Title = "Quest#3", Status = "Complete", Created = new DateTime(2023, 2, 26, 10, 25, 30), Description = "Description#3", Modified = DateTime.Now };
        }
    }
}
