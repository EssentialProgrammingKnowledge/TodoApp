namespace TodoAppConsole
{
    public class QuestIteractionService
    {
        public Quest? CreateQuest()
        {
            var quest = new Quest
            {
                Status = QuestStatus.New
            };
            if (!SetTitle(quest))
            {
                return null;
            }
            SetDescription(quest);
            quest.Created = DateTime.Now;
            return quest;
        }

        public Quest? ModifiedQuest(Quest quest)
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

            quest.Status = (QuestStatus)status;
            quest.Modified = DateTime.Now;
            return quest;
        }
        public int GetQuestId()
        {
            Console.WriteLine("Please enter quest id");
            int.TryParse(Console.ReadLine(), out var id);
            return id;
        }

        private bool SetTitle(Quest quest)
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

        private void SetDescription(Quest quest)
        {
            Console.WriteLine("Enter quest description");
            quest.Description = Console.ReadLine() ?? "";
        }
    }
}
