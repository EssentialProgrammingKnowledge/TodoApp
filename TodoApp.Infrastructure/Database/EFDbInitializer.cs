using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Infrastructure.Database
{
    internal class EFDbInitializer : IDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public EFDbInitializer(IServiceProvider serviceProvider)
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

            using var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            context.Database.Migrate();
        }
    }
}
