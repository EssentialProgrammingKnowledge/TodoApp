using TodoApp.Core.Services;

namespace TodoAppConsole
{
    internal class GetQuestView : IConsoleView
    {
        private readonly IQuestService _questService;

        public GetQuestView(IQuestService questService)
        {
            _questService = questService;
        }

        public async Task View()
        {
            var id = GetQuestId();
            var quest = await _questService.GetQuestById(id);

            if (quest is null)
            {
                Console.WriteLine($"Quest with id {id} was not found");
            }
            else
            {
                Console.WriteLine($"Quest: {quest}");
            }
        }

        private static int GetQuestId()
        {
            Console.WriteLine("Please enter quest id");
            int.TryParse(Console.ReadLine(), out var id);
            return id;
        }
    }
}
