using TodoApp.Core.DTO;
using TodoApp.Core.Services;

namespace TodoAppConsole
{
    internal class AddQuestView : IConsoleView
    {
        private readonly IQuestService _questService;

        public AddQuestView(IQuestService questService)
        {
            _questService = questService;
        }

        public async Task View()
        {
            var quest = CreateQuest();
            if (quest is null)
            {
                return;
            }
            quest = await _questService.AddQuest(quest);
            Console.WriteLine($"Added quest {quest}");
        }

        private QuestDto? CreateQuest()
        {
            var quest = new QuestDto();
            if (!SetTitle(quest))
            {
                return null;
            }
            SetDescription(quest);
            return quest;
        }

        private static bool SetTitle(QuestDto quest)
        {
            Console.WriteLine("Enter quest title");
            var title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty");
                return false;
            }
            quest.Title = title;
            return true;
        }

        private static void SetDescription(QuestDto quest)
        {
            Console.WriteLine("Enter quest description");
            quest.Description = Console.ReadLine() ?? "";
        }
    }
}
