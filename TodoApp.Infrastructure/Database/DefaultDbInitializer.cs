using FluentMigrator.Runner;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace TodoApp.Infrastructure.Database
{
    internal sealed class DefaultDbInitializer : IDbInitializer
    {
        private readonly DatabaseOptions _databaseOptions;
        private readonly IMigrationRunner _migrationRunner;

        public DefaultDbInitializer(IOptions<DatabaseOptions> databaseOptions, IMigrationRunner migrationRunner)
        {
            _databaseOptions = databaseOptions.Value;
            _migrationRunner = migrationRunner;
        }

        public async Task Start()
        {
            if (!_databaseOptions.AllowMigrations)
            {
                return;
            }

            await CreateDatabaseIfNotExists(_databaseOptions.ConnectionString!);
            _migrationRunner.MigrateUp();
        }

        private static async Task CreateDatabaseIfNotExists(string connectionString)
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
            await conn.OpenAsync();
            cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{database}`";
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
