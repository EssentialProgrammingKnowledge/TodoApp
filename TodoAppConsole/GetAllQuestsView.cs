using TodoApp.Core.Services;

namespace TodoAppConsole
{
    internal class GetAllQuestsView : IConsoleView
    {
        private readonly IQuestService _questService;

        public GetAllQuestsView(IQuestService questService)
        {
            _questService = questService;
        }

        public void View()
        {
            var quests = _questService.GetAllQuests();
            foreach (var quest in quests)
            {
                Console.WriteLine(quest);
            }
        }
    }
}
