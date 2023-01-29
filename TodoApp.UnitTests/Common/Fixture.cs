using TodoApp.Core.Entities;

namespace TodoApp.UnitTests.Common
{
    internal static class Fixture
    {
        public static Quest CreateDefaultQuest(int id = 1, string? title = null, string? description = null, QuestStatus questStatus = QuestStatus.New)
        {
            return new Quest(id, title ?? $"Title#{Guid.NewGuid():N}",
                description ?? "", questStatus, DateTime.UtcNow);
        }
    }
}
