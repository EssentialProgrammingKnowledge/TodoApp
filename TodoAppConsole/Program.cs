using TodoAppConsole;

static List<Menu> CreateMenus()
{
    return new List<Menu>()
    {
        new Menu { Id = 1, Name = "Add quest" },
        new Menu { Id = 2, Name = "Show details" },
        new Menu { Id = 3, Name = "Update quest" },
        new Menu { Id = 4, Name = "Show all quests" },
        new Menu { Id = 5, Name = "Delete quest" },
        new Menu { Id = 6, Name = "Quit program" },
    };
}
static void ShowMenus(IEnumerable<Menu> menus)
{
    Console.WriteLine("Please choose menu:");
    foreach (var menu in menus)
    {
        Console.WriteLine(menu);
    }
}

var menuService = new MenuService(CreateMenus());
var questService = new QuestService();
var questInteractionService = new QuestIteractionService();

Quest? GetQuest()
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

    switch (consoleKey.Key)
    {
        case ConsoleKey.D1:
            var quest = questInteractionService.CreateQuest();

            if (quest is null)
            {
                break;
            }

            questService.AddQuest(quest);
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
            questService.UpdateQuest(quest);
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
            var deleted = questService.DeleteQuest(id);
            if (deleted)
            {
                Console.WriteLine($"Quest with id: {id} was deleted");
            }
            else
            {
                Console.WriteLine($"Quest with id {id} was not deleted");
            }
            break;
        case ConsoleKey.D6:
            isProgramRunning = false;
            break;
        default:
            Console.WriteLine("Entered invalid Key");
            break;
    }
}