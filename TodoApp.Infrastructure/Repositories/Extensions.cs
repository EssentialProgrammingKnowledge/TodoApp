using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;
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
                var options = sp.GetRequiredService<DatabaseOptions>();
                return new MySqlConnection(options.ConnectionString!);
            });
            services.AddScoped<IRepository<Quest>, DapperQuestRepository>();
            return services;
        }
    }
}
