namespace TodoAppConsole
{
    public class MenuService
    {
        private readonly IEnumerable<Menu> _menus;

        public MenuService(IEnumerable<Menu> menus)
        {
            _menus = menus;
        }

        public IEnumerable<Menu> GetMenus() => _menus;

        public Menu? GetMenuById(int id)
        {
            foreach (var menu in _menus)
            {
                if (menu.Id == id)
                {
                    return menu;
                }
            }

            return null;
        }
    }
}
