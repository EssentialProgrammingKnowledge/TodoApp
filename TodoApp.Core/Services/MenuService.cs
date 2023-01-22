using TodoApp.Core.DTO;
using TodoApp.Core.Entities;
using TodoApp.Core.Mappings;

namespace TodoApp.Core.Services
{
    internal sealed class MenuService : IMenuService
    {
        private readonly IEnumerable<Menu> _menus;

        public MenuService(IEnumerable<MenuDto> menuDtos)
        {
            var menus = new List<Menu>();
            foreach (var menu in menuDtos)
            {
                menus.Add(menu.AsEntity());
            }
            _menus = menus;
        }

        public IEnumerable<MenuDto> GetMenus()
        {
            var menuList = new List<MenuDto>();
            foreach (var menu in _menus)
            {
                menuList.Add(menu.AsDto());
            }
            return menuList;
        }

        public MenuDto? GetMenuById(int id)
        {
            foreach (var menu in _menus)
            {
                if (menu.Id == id)
                {
                    return menu.AsDto();
                }
            }

            return null;
        }
    }
}
