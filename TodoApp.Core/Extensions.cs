using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TodoApp.Core.DTO;
using TodoApp.Core.Services;

[assembly: InternalsVisibleTo("TodoApp.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace TodoApp.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddSingleton<IEnumerable<MenuDto>>(_ => CreateMenus());
            services.AddSingleton<IMenuService, MenuService>();
            services.AddScoped<IQuestService, QuestService>();
            return services;
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