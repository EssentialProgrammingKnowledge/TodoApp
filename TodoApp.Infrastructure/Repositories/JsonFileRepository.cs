using System.Reflection;
using System.Text.Json;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;
using TodoApp.Infrastructure.Repositories.Files;

namespace TodoApp.Infrastructure.Repositories
{
    internal sealed class JsonFileRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly string _filePathEntities;
        private readonly IPrimaryKeyManager<T> _primaryKeyManager;
        private readonly IPrimaryKeyPositionCache<T> _primaryKeyPositionCache;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonFileRepository(IPrimaryKeyManager<T> primaryKeyManager, IPrimaryKeyPositionCache<T> primaryKeyPositionCache)
        {
            var type = typeof(T);
            _filePathEntities = Directory.GetParent(Environment.CurrentDirectory)!
                        .Parent!.Parent!.FullName + Path.DirectorySeparatorChar + type.Name + ".json";
            _primaryKeyManager = primaryKeyManager;
            _primaryKeyPositionCache = primaryKeyPositionCache;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                TypeInfoResolver = new PrivateConstructorContractResolver()
            };

            if (!File.Exists(_filePathEntities))
            {
                using FileStream fileStream = File.Open(_filePathEntities, FileMode.CreateNew, FileAccess.Write);
            }
        }

        public async Task<int> Add(T entity)
        {
            var id = await _primaryKeyManager.GetNextPrimaryKey();
            SetId(entity, id);
            var lastPosition = _primaryKeyPositionCache.GetLastPosition();
            var nextPosition = lastPosition + 1;
            string jsonString = JsonSerializer.Serialize(new FileEntity<T> { Position = nextPosition, Entity = entity });
            await File.AppendAllLinesAsync(_filePathEntities, new string[] { jsonString });
            await _primaryKeyPositionCache.AddPosition(id, nextPosition);
            return id;
        }

        public async Task Delete(T entity)
        {
            var position = _primaryKeyPositionCache.GetPosition(entity.Id);
            var tempFile = Path.GetTempFileName();
            // get lines that not contains entity to delete
            var linesToKeep = File.ReadLines(_filePathEntities)
                .Where((_, index) => index != position - 1)
                .Select((line, index) =>
                {
                    var fileEntity = JsonSerializer.Deserialize<FileEntity<T>>(line, _jsonSerializerOptions);
                    return JsonSerializer.Serialize(new FileEntity<T> { Entity = fileEntity.Entity, Position = index + 1 });
                });

            // write to temp file and delete current then move from temp to current localization
            await File.WriteAllLinesAsync(tempFile, linesToKeep);
            File.Delete(_filePathEntities);
            File.Move(tempFile, _filePathEntities);

            // update primary keys positions in cache
            var primaryKeyPositions = File.ReadLines(_filePathEntities)
               .Select((line) =>
               {
                   var fileEntity = JsonSerializer.Deserialize<FileEntity<T>>(line, _jsonSerializerOptions);
                   return new PrimaryKeyPosition { Id = fileEntity.Entity.Id, Position = fileEntity.Position };
               });
            await _primaryKeyPositionCache.UpdatePositions(primaryKeyPositions);
        }

        public async Task<T?> Get(int id)
        {
            await Task.CompletedTask;
            var position = _primaryKeyPositionCache.GetPosition(id);

            if (position - 1 == -1)
            {
                return null;
            }

            var line = File.ReadLines(_filePathEntities)
                .Where((_, index) => index == position - 1)
                .SingleOrDefault();

            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            return JsonSerializer.Deserialize<FileEntity<T>>(line, _jsonSerializerOptions)?.Entity ?? null;
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            var list = new List<T>();
            await foreach(var line in File.ReadLinesAsync(_filePathEntities))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var fileEntity = JsonSerializer.Deserialize<FileEntity<T>>(line, _jsonSerializerOptions);
                if (fileEntity is null || fileEntity.Entity is null)
                {
                    continue;
                }
                list.Add(fileEntity.Entity);
            }
            return list;
        }

        public async Task Update(T entity)
        {
            var position = _primaryKeyPositionCache.GetPosition(entity.Id);
            var tempFile = Path.GetTempFileName();
            // update line with new values
            var lines = File.ReadLines(_filePathEntities)
                .Select((line, index) =>
                {
                    if (index == position - 1)
                    {
                        return JsonSerializer.Serialize(new FileEntity<T>
                        {
                            Position = position,
                            Entity = entity
                        });
                    }
                    return line;
                });
            // write to temp file, delete current and move to current localization
            await File.WriteAllLinesAsync(tempFile, lines);
            File.Delete(_filePathEntities);
            File.Move(tempFile, _filePathEntities);
        }

        private static void SetId(T entity, int id)
        {
            var type = typeof(T);
            var field = type?.BaseType?.GetField($"<{nameof(BaseEntity.Id)}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(entity, id);
        }
    }
}
