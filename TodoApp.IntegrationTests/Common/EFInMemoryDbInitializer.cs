using TodoApp.Infrastructure.Database;

namespace TodoApp.IntegrationTests.Common
{
    internal sealed class EFInMemoryDbInitializer : IDbInitializer
    {
        private readonly TodoDbContext _todoDbContext;

        public EFInMemoryDbInitializer(TodoDbContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        public async Task Start()
        {
            await _todoDbContext.Database.EnsureCreatedAsync();
        }
    }
}
