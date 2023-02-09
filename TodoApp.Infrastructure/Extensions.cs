using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Repositories.Files;

namespace TodoApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<>), typeof(JsonFileRepository<>));
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

            foreach(var type in types)
            {
                var primaryKeyMangerType = typeof(IPrimaryKeyManager<>).MakeGenericType(type);
                var primaryKeyPositionCache = typeof(IPrimaryKeyPositionCache<>).MakeGenericType(type);
                serviceProvider.GetRequiredService(primaryKeyMangerType);
                serviceProvider.GetRequiredService(primaryKeyPositionCache);
            }
            return serviceProvider;
        }
    }
}