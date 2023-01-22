using TodoApp.Core.DTO;

namespace TodoApp.Core.Services
{
    public interface IMenuService : IService<MenuDto>
    {
        IEnumerable<MenuDto> GetMenus();

        MenuDto? GetMenuById(int id);
    }
}
