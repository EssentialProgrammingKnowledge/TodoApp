using System.Reflection;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;

namespace TodoApp.Infrastructure.Repositories
{
    internal sealed class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly Dictionary<string, List<T>> _entities = new();

        public Task<int> Add(T entity)
        {
            var type = typeof(T);
            var containsList = _entities.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                list = new List<T>() { entity };
                SetId(entity, list);
                _entities.Add(type.Name, list);
                return Task.FromResult(entity.Id);
            }

            SetId(entity, list!);
            list!.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public Task Delete(T entity)
        {
            var type = typeof(T);
            _entities.TryGetValue(type.Name, out var list);
            list?.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<T?> Get(int id)
        {
            var type = typeof(T);
            var containsList = _entities.TryGetValue(type.Name, out var list);
            return Task.FromResult(list?.SingleOrDefault(t => t.Id == id));
        }

        public Task<IReadOnlyList<T>> GetAll()
        {
            var type = typeof(T);
            _entities.TryGetValue(type.Name, out var list);
            return Task.FromResult<IReadOnlyList<T>>(list ?? new List<T>());
        }

        public Task Update(T entity)
        {
            return Task.CompletedTask;
        }

        private static void SetId(T entity, List<T> list)
        {
            var type = typeof(T);
            var field = type?.BaseType?.GetField($"<{nameof(BaseEntity.Id)}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            var lastId = list?.LastOrDefault()?.Id ?? 0;
            field?.SetValue(entity, lastId + 1);
        }
    }
}
