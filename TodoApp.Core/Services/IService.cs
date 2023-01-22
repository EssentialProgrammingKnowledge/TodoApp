using TodoApp.Core.DTO;

namespace TodoApp.Core.Services
{
    public interface IService<T>
        where T : class, IBaseDto
    {
    }
}
