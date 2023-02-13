using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Migrations;

namespace TodoApp.Infrastructure.Database
{
    internal static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, Dictionary<string, object> appsettings)
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
            var serverVersion = ServerVersion.AutoDetect(databaseOptions.ConnectionString);
            services.AddDbContext<TodoDbContext>(options => options.UseMySql(databaseOptions.ConnectionString, serverVersion));
            services.AddEFCoreRepositories();

            if (databaseOptions.AllowMigrations)
            {
                services.AddTransient<IDbInitializer, EFDbInitializer>();
            }

            return services;
        }
    }
}
