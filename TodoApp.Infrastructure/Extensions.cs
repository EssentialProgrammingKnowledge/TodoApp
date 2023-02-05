using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.Repositories;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}