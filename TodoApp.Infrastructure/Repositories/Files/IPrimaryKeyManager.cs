
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Repositories.Files
{
    internal interface IPrimaryKeyManager<T>
        where T : BaseEntity
    {
        Task<int> GetNextPrimaryKey();
    }
}
