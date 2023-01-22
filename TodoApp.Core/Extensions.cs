using TodoApp.Core.DTO;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;

namespace TodoApp.Core
{
    public static class Extensions
    {
        public static IMenuService GetMenuService()
        {
            return new MenuService(CreateMenus());
        }

        public static IQuestService GetQuestService()
        {
            var repository = new Repository<Quest>();
            return new QuestService(repository);
        }

        private static List<MenuDto> CreateMenus()
        {
            return new List<MenuDto>()
            {
                new MenuDto { Id = 1, Name = "Add quest" },
                new MenuDto { Id = 2, Name = "Show details" },
                new MenuDto { Id = 3, Name = "Update quest" },
                new MenuDto { Id = 4, Name = "Show all quests" },
                new MenuDto { Id = 5, Name = "Delete quest" },
                new MenuDto { Id = 6, Name = "Quit program" },
            };
        }
    }
}