using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace TodoApp.Infrastructure.Database
{
    internal sealed class DbInitializer : IDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DbInitializer(IServiceProvider serviceProvider)
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

            CreateDatabaseIfNotExists(databaseOptions.ConnectionString!);
            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();
        }

        private void CreateDatabaseIfNotExists(string connectionString)
        {
            var connectionStringSplited = connectionString.Split(";");
            var connectionStringWithoutDb = connectionStringSplited.Where(str => !str.Contains("Database="))
                                .Aggregate((current, next) => current + ";" + next);
            var database = connectionStringSplited.SingleOrDefault(str => str.Contains("Database="))?.Split("Database=")[1];

            if (string.IsNullOrEmpty(database))
            {
                throw new InvalidOperationException("Invalid ConnectionString. There is no value for 'Database=' check it and try again");
            }

            using var conn = new MySqlConnection(connectionStringWithoutDb);
            using var cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{database}`";
            cmd.ExecuteNonQuery();
        }
    }
}
