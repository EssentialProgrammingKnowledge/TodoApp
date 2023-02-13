using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Infrastructure.Database;

namespace TodoApp.IntegrationTests
{
    internal sealed class DbTestInitializer : IDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DbTestInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Start()
        {
            using var scope = _serviceProvider.CreateScope();
            var databaseOptions = scope.ServiceProvider.GetRequiredService<DatabaseOptions>();

            if (!databaseOptions.AllowMigrations)
            {
                return;
            }

            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();
        }
    }
}
