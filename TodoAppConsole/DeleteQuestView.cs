using TodoApp.Core.Services;

namespace TodoAppConsole
{
    internal class DeleteQuestView : IConsoleView
    {
        private readonly IQuestService _questService;

        public DeleteQuestView(IQuestService questService)
        {
            _questService = questService;
        }

        public void View()
        {
            var id = GetQuestId();
            _questService.DeleteQuest(id);
            Console.WriteLine($"Quest with id: {id} was deleted");
        }

        private static int GetQuestId()
        {
            Console.WriteLine("Please enter quest id");
            int.TryParse(Console.ReadLine(), out var id);
            return id;
        }
    }
}
