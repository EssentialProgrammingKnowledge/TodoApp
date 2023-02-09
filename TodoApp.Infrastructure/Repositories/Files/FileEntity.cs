using TodoApp.Core.Entities;

namespace TodoApp.Infrastructure.Repositories.Files
{
    internal class FileEntity<T>
        where T : BaseEntity
    {
        public int Position { get; set; }
        public T Entity { get; set; }
    }
}
