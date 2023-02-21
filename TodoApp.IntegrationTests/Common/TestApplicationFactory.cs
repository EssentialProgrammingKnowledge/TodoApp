using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Infrastructure.Database;

namespace TodoApp.IntegrationTests.Common
{
    public sealed class TestApplicationFactory : WebApplicationFactory<Program>
    {
        public HttpClient Client { get; }

        public TestApplicationFactory()
        {
            Client = CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("test");
        }

        public override async ValueTask DisposeAsync()
        {
            var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
            scope.Dispose();
            await base.DisposeAsync();
        }
    }
}
