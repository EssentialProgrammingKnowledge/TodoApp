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
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"DROP DATABASE IF EXISTS {connection.Database}";
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            scope.Dispose();
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
