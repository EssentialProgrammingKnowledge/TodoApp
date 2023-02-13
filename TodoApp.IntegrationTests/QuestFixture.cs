using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Text.Json;
using TodoApp.Core;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Database;

namespace TodoApp.IntegrationTests
{
    public class QuestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; set; }

        public QuestFixture()
        {
            ServiceProvider = new ServiceCollection().AddCore()
                                .AddInfrastructure(GetAppsettings())
                                .BuildServiceProvider()
                                .UseInfrastructure();
        }

        private Dictionary<string, object> GetAppsettings()
        {
            var appFolder = AppContext.BaseDirectory
                ?? throw new InvalidOperationException("Cannot get Application folder directory");
            using FileStream fileStream = File.Open(appFolder + Path.DirectorySeparatorChar + "appsettings.json", FileMode.Open, FileAccess.Read);
            var settings = JsonSerializer.Deserialize<Dictionary<string, object>>(fileStream)
                    ?? throw new InvalidOperationException("Cannot deserialize 'appsettings.json', please ensure that this file exists");
            return settings;
        }

        public void Dispose()
        {
            using var scope = ServiceProvider.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            dbContext.Database.EnsureDeleted();
        }
    }
}
