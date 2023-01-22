using TodoApp.Core.Entities;

namespace TodoApp.Core.Repositories
{
    internal interface IRepository<T>
        where T : BaseEntity
    {
        int Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T? Get(int id);
        IReadOnlyList<T> GetAll();
    }
}
