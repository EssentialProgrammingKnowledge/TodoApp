using System.Text.Json;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Repositories.Files
{
    internal class PrimaryKeyManager<T> : IPrimaryKeyManager<T>
        where T : BaseEntity
    {
        private readonly string _filePath;

        public PrimaryKeyManager()
        {
            var type = typeof(T);
            _filePath = Directory.GetParent(Environment.CurrentDirectory)!
                        .Parent!.Parent!.FullName + Path.DirectorySeparatorChar + type.Name + "_auto_increment.json";

            if (!File.Exists(_filePath))
            {
                using FileStream fileStream = File.Open(_filePath, FileMode.CreateNew, FileAccess.Write);
                JsonSerializer.Serialize(fileStream, new PrimaryKey { Id = 0 });
            }
        }

        public async Task<int> GetNextPrimaryKey()
        {
            var lastIndex = await GetIndexFromFileAsync();
            using FileStream fileStream = File.Open(_filePath, FileMode.Create, FileAccess.Write);
            var nextId = lastIndex + 1;
            await JsonSerializer.SerializeAsync(fileStream, new PrimaryKey() { Id = nextId });
            return nextId;
        }

        private async Task<int> GetIndexFromFileAsync()
        {
            using FileStream fileStreamRead = File.Open(_filePath, FileMode.Open, FileAccess.Read);
            var primaryKey = await JsonSerializer.DeserializeAsync<PrimaryKey>(fileStreamRead);
            return primaryKey!.Id;
        }

    }
}
