using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;
using TodoApp.Infrastructure.Database;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Repositories.Files;
using TodoApp.Migrations;

namespace TodoApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            //services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<>), typeof(JsonFileRepository<>));
            services.AddScoped<IRepository<Quest>, QuestRepository>();
            services.AddDatabase(appsettings);
            services.AddSingleton(typeof(IPrimaryKeyManager<>), typeof(PrimaryKeyManager<>));
            services.AddSingleton(typeof(IPrimaryKeyPositionCache<>), typeof(PrimaryKeyPositionCache<>));
            return services;
        }

        public static IServiceProvider UseInfrastructure(this IServiceProvider serviceProvider)
        {
            var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.Start();
            return serviceProvider;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            var databaseOptionsJsonElement = (JsonElement)(appsettings["database"]
                ?? throw new InvalidOperationException("Cannot find section 'database', please ensure that this file exists"));
            var databaseOptions = databaseOptionsJsonElement.Deserialize<DatabaseOptions>(new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            }) ?? throw new InvalidOperationException("Cannot deserialize section 'database', please ensure that this section is correct");
            if (string.IsNullOrWhiteSpace(databaseOptions.ConnectionString))
            {
                throw new InvalidOperationException("Check in 'database' section 'connectionString' if is correct");
            }
            services.AddSingleton(databaseOptions);
            services.AddTransient<IDbInitializer, DbInitializer>();

            if (databaseOptions.AllowMigrations)
            {
                services.AddMigrations(databaseOptions.ConnectionString);
            }

            services.AddScoped<IDbConnection, MySqlConnection>(sp =>
            {
                return new MySqlConnection(databaseOptions.ConnectionString);
            });
            return services;
        }
    }
}