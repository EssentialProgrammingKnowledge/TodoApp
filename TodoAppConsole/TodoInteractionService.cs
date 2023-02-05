using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.DTO;
using TodoApp.Core.Services;

namespace TodoAppConsole
{
    internal class TodoInteractionService : ITodoInteractionService
    {
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope? serviceScope;

        public TodoInteractionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Start()
        {
            var menuService = _serviceProvider.GetRequiredService<IMenuService>();
            RunConsoleApp(menuService.GetMenus().ToList());
        }

        private void RunConsoleApp(IEnumerable<MenuDto> menus)
        {
            bool isProgramRunning = true;
            while (isProgramRunning)
            {
                ShowMenus(menus);
                var consoleKey = Console.ReadKey();
                Console.WriteLine();

                try
                {
                    InitializeScope();
                    switch (consoleKey.Key)
                    {
                        case ConsoleKey.D1:
                            var addQuestView = serviceScope!.ServiceProvider.GetRequiredService<AddQuestView>();
                            addQuestView.View();
                            break;
                        case ConsoleKey.D2:
                            var questView = serviceScope!.ServiceProvider.GetRequiredService<GetQuestView>();
                            questView.View();
                            break;
                        case ConsoleKey.D3:
                            var updateQuestView = serviceScope!.ServiceProvider.GetRequiredService<UpdateQuestView>();
                            updateQuestView.View();
                            break;
                        case ConsoleKey.D4:
                            var questsView = serviceScope!.ServiceProvider.GetRequiredService<GetAllQuestsView>();
                            questsView.View();
                            break;
                        case ConsoleKey.D5:
                            var deleteQuestView = serviceScope!.ServiceProvider.GetRequiredService<DeleteQuestView>();
                            deleteQuestView.View();
                            break;
                        case ConsoleKey.D6:
                            isProgramRunning = false;
                            break;
                        default:
                            Console.WriteLine("Entered invalid Key");
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    DisposeScope();
                }
            }
        }

        private void InitializeScope()
        {
            serviceScope = _serviceProvider.CreateScope();
        }

        private void DisposeScope()
        {
            serviceScope?.Dispose();
            serviceScope = null;
        }

        private static void ShowMenus(IEnumerable<MenuDto> menus)
        {
            Console.WriteLine("Please choose menu:");
            foreach (var menu in menus)
            {
                Console.WriteLine(menu);
            }
        }
    }
}
