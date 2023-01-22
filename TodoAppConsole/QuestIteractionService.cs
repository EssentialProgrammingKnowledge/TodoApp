using TodoApp.Core.DTO;
using TodoApp.Core.Entities;

namespace TodoAppConsole
{
    public class QuestIteractionService
    {
        public QuestDto? CreateQuest()
        {
            var quest = new QuestDto();
            if (!SetTitle(quest))
            {
                return null;
            }
            SetDescription(quest);
            return quest;
        }

        public QuestDto? ModifiedQuest(QuestDto quest)
        {
            if (!SetTitle(quest))
            {
                return null;
            }
            SetDescription(quest);
            Console.WriteLine("Enter quest status: 0 - New, 1 - InProgress, 2 - Complete");
            var parsed = int.TryParse(Console.ReadLine(), out var status);

            if (!parsed)
            {
                Console.WriteLine($"Entered invalid status");
                return null;
            }

            if (status > 2 || status < 0)
            {
                Console.WriteLine($"Entered invalid status {status}");
                return null;
            }

            quest.Status = ((QuestStatus)status).ToString();
            return quest;
        }
        public int GetQuestId()
        {
            Console.WriteLine("Please enter quest id");
            int.TryParse(Console.ReadLine(), out var id);
            return id;
        }

        private bool SetTitle(QuestDto quest)
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

        private void SetDescription(QuestDto quest)
        {
            Console.WriteLine("Enter quest description");
            quest.Description = Console.ReadLine() ?? "";
        }
    }
}
