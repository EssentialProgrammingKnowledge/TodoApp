using TodoApp.Core.Entities;

namespace TodoApp.Core.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<int> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T?> Get(int id);
        Task<IReadOnlyList<T>> GetAll();
    }
}
