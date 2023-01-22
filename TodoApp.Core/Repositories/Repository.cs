using TodoApp.Core.Entities;

namespace TodoApp.Core.Repositories
{
    internal sealed class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly Dictionary<string, List<T>> _entities = new();

        public int Add(T entity)
        {
            var type = typeof(T);
            var containsList = _entities.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                entity.Id = 1;
                list = new List<T>() { entity };
                _entities.Add(type.Name, list);
                return entity.Id;
            }

            entity.Id = list![^1].Id + 1;
            list.Add(entity);
            return entity.Id;
        }

        public void Delete(T entity)
        {
            var type = typeof(T);
            _entities.TryGetValue(type.Name, out var list);
            list?.Remove(entity);
        }

        public T? Get(int id)
        {
            var type = typeof(T);
            var containsList = _entities.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                return null;
            }

            foreach (var item in list!)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return null;
        }

        public IReadOnlyList<T> GetAll()
        {
            var type = typeof(T);
            _entities.TryGetValue(type.Name, out var list);
            return list ?? new List<T>();
        }

        public void Update(T entity)
        {
        }
    }
}
