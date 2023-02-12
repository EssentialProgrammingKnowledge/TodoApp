using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Repositories.Files;

namespace TodoApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            //services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<>), typeof(JsonFileRepository<>));
            services.AddScoped<IRepository<Quest>, QuestRepository>();
            services.AddDbConnection(connectionString);
            services.AddSingleton(typeof(IPrimaryKeyManager<>), typeof(PrimaryKeyManager<>));
            services.AddSingleton(typeof(IPrimaryKeyPositionCache<>), typeof(PrimaryKeyPositionCache<>));
            return services;
        }

        public static IServiceProvider UseInfrastructure(this IServiceProvider serviceProvider)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes()
                            .Where(t => typeof(BaseEntity)
                                    .IsAssignableFrom(t) && t != typeof(BaseEntity)
                                        && t != typeof(Menu)));

            foreach (var type in types)
            {
                var primaryKeyMangerType = typeof(IPrimaryKeyManager<>).MakeGenericType(type);
                var primaryKeyPositionCache = typeof(IPrimaryKeyPositionCache<>).MakeGenericType(type);
                serviceProvider.GetRequiredService(primaryKeyMangerType);
                serviceProvider.GetRequiredService(primaryKeyPositionCache);
            }
            return serviceProvider;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IDbConnection, MySqlConnection>(sp =>
            {
                return new MySqlConnection(connectionString);
            });
            return services;
        }
    }
}