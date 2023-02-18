using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;
using TodoApp.Infrastructure.Database;
using TodoApp.Infrastructure.Repositories.Files;

namespace TodoApp.Infrastructure.Repositories
{
    internal static class Extensions
    {
        public static IServiceCollection AddInMemoryRepositories(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));
            return services;
        }

        public static IServiceCollection AddFileRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(JsonFileRepository<>));
            services.AddSingleton(typeof(IPrimaryKeyManager<>), typeof(PrimaryKeyManager<>));
            services.AddSingleton(typeof(IPrimaryKeyPositionCache<>), typeof(PrimaryKeyPositionCache<>));
            return services;
        }

        public static IServiceCollection AddDapperRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDbConnection, MySqlConnection>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<DatabaseOptions>>();
                return new MySqlConnection(options.Value.ConnectionString!);
            });
            services.AddScoped<IRepository<Quest>, DapperQuestRepository>();
            return services;
        }

        public static IServiceCollection AddEFCoreRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Quest>, EFQuestRepository>();
            return services;
        }
    }
}
