using TodoApp.Core;
using TodoApp.Core.DTO;
using TodoAppConsole;

static void ShowMenus(IEnumerable<MenuDto> menus)
{
    Console.WriteLine("Please choose menu:");
    foreach (var menu in menus)
    {
        Console.WriteLine(menu);
    }
}

var menuService = Extensions.GetMenuService();
var questService = Extensions.GetQuestService();
var questInteractionService = new QuestIteractionService();

QuestDto? GetQuest()
{
    var id = questInteractionService.GetQuestId();
    var quest = questService.GetQuestById(id);

    if (quest is null)
    {
        Console.WriteLine($"Quest with id {id} was not found");
    }
    return quest;
}

var menus = menuService.GetMenus();

bool isProgramRunning = true;
while (isProgramRunning)
{
    ShowMenus(menus);
    var consoleKey = Console.ReadKey();
    Console.WriteLine();

    try
    {
        switch (consoleKey.Key)
        {
            case ConsoleKey.D1:
                var quest = questInteractionService.CreateQuest();

                if (quest is null)
                {
                    break;
                }

                quest = questService.AddQuest(quest);
                Console.WriteLine($"Added quest {quest}");
                break;
            case ConsoleKey.D2:
                quest = GetQuest();
                if (quest is null)
                {
                    break;
                }
                Console.WriteLine($"Quest: {quest}");
                break;
            case ConsoleKey.D3:
                quest = GetQuest();
                if (quest is null)
                {
                    break;
                }
                questInteractionService.ModifiedQuest(quest);
                quest = questService.UpdateQuest(quest);
                Console.WriteLine($"Updated quest {quest}");
                break;
            case ConsoleKey.D4:
                var quests = questService.GetAllQuests();
                foreach (var questInList in quests)
                {
                    Console.WriteLine($"Quest: {questInList}");
                }
                break;
            case ConsoleKey.D5:
                var id = questInteractionService.GetQuestId();
                questService.DeleteQuest(id);
                Console.WriteLine($"Quest with id: {id} was deleted");
                break;
            case ConsoleKey.D6:
                isProgramRunning = false;
                break;
            default:
                Console.WriteLine("Entered invalid Key");
                break;
        }
    }
    catch(Exception exception)
    {
        Console.WriteLine(exception.Message);
    }
}