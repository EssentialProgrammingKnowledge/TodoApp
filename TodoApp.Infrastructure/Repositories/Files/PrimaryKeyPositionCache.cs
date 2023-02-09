using System.Text.Json;

namespace TodoApp.Infrastructure.Repositories.Files
{
    internal class PrimaryKeyPositionCache<T> : IPrimaryKeyPositionCache<T>
        where T : class
    {
        private readonly string _filePath;
        private IDictionary<string, List<PrimaryKeyPosition>> primaryKeyPositions;

        public PrimaryKeyPositionCache()
        {
            var type = typeof(T);
            _filePath = Directory.GetParent(Environment.CurrentDirectory)!
                        .Parent!.Parent!.FullName + Path.DirectorySeparatorChar + type.Name + "_key_positions.json";
            primaryKeyPositions = new Dictionary<string, List<PrimaryKeyPosition>>();
            GetAllPositions();
        }

        private void GetAllPositions()
        {
            if (!File.Exists(_filePath))
            {
                using FileStream fileStreamWrite = File.Open(_filePath, FileMode.Create, FileAccess.Write);
                JsonSerializer.Serialize(fileStreamWrite, primaryKeyPositions);
                return;
            }
            
            using FileStream fileStreamRead = File.Open(_filePath, FileMode.Open, FileAccess.Read);
            primaryKeyPositions = JsonSerializer.Deserialize<Dictionary<string, List<PrimaryKeyPosition>>>(fileStreamRead)
                                    ?? new Dictionary<string, List<PrimaryKeyPosition>>();
        }

        public async Task AddPosition(int key, int position)
        {
            var type = typeof(T);
            var containsList = primaryKeyPositions.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                list = new List<PrimaryKeyPosition>()
                {
                    new PrimaryKeyPosition{ Id = key, Position = position }
                };
                primaryKeyPositions.Add(type.Name, list);
                await SerializePrimaryKeyPositions();
                return;
            }

            list!.Add(new PrimaryKeyPosition { Id = key, Position = position });
            await SerializePrimaryKeyPositions();
        }

        public int GetPosition(int key)
        {
            var type = typeof(T);
            var containsList = primaryKeyPositions.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                return default;
            }

            return list?.SingleOrDefault(list => list.Id == key)?.Position ?? default;
        }

        public int GetLastPosition()
        {
            var type = typeof(T);
            var containsList = primaryKeyPositions.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                return default;
            }

            return list?.LastOrDefault()?.Position ?? default;
        }

        public async Task UpdatePositions(IEnumerable<PrimaryKeyPosition> primaryKeys)
        {
            var type = typeof(T);
            var containsList = primaryKeyPositions.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                list = new List<PrimaryKeyPosition>(primaryKeys);
                primaryKeyPositions.Add(type.Name, list);
                await SerializePrimaryKeyPositions();
                return;
            }

            list!.RemoveAll((_) => true);
            list!.AddRange(primaryKeys);
            await SerializePrimaryKeyPositions();

        }

        private async Task SerializePrimaryKeyPositions()
        {
            using FileStream fileStreamWrite = File.Open(_filePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(fileStreamWrite, primaryKeyPositions);
        }
    }
}
