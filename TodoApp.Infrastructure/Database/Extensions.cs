using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Infrastructure.Database
{
    internal static class Extensions
    {
        #region ConsoleApp

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

            services.AddSingleton(Options.Create(databaseOptions));
            var serverVersion = ServerVersion.AutoDetect(databaseOptions.ConnectionString);
            services.AddDbContext<TodoDbContext>(options => options.UseMySql(databaseOptions.ConnectionString, serverVersion));
            services.AddEFCoreRepositories();
            services.AddScoped<IDbInitializer, EFDbInitializer>();
            services.AddHostedService<DbInitializer>();
            return services;
        }

        #endregion

        #region WebApi

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptionsSection = configuration.GetSection("database");
            services.Configure<DatabaseOptions>(databaseOptionsSection);
            var databaseOptions = new DatabaseOptions();
            databaseOptionsSection.Bind(databaseOptions);
            var serverVersion = ServerVersion.AutoDetect(databaseOptions.ConnectionString);
            services.AddDbContext<TodoDbContext>(options => options.UseMySql(databaseOptions.ConnectionString, serverVersion));
            services.AddEFCoreRepositories();
            services.AddScoped<IDbInitializer, EFDbInitializer>();
            services.AddHostedService<DbInitializer>();
            return services;
        }

        #endregion
    }
}
