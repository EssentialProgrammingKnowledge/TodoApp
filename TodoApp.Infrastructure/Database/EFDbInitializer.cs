using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TodoApp.Infrastructure.Database
{
    internal class EFDbInitializer : IDbInitializer
    {
        private readonly DatabaseOptions _databaseOptions;
        private readonly TodoDbContext _todoDbContext;

        public EFDbInitializer(IOptions<DatabaseOptions> databaseOptions, TodoDbContext todoDbContext)
        {
            _databaseOptions = databaseOptions.Value;
            _todoDbContext = todoDbContext;
        }

        public async Task Start()
        {
            if (!_databaseOptions.AllowMigrations)
            {
                return;
            }

            await _todoDbContext.Database.MigrateAsync();
        }
    }
}
