using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Infrastructure.Database;

namespace TodoApp.IntegrationTests.Common
{
    public sealed class TestApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(SetupEFCoreInMemory);
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

        private static void SetupEFCoreInMemory(IServiceCollection services)
        {
            // remove implementation of infrastructure dbContext
            var contextDb = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TodoDbContext));
            if (contextDb != null)
            {
                services.Remove(contextDb);
                var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                  || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToList();

                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }
            services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("TodoDbApp"));
            services.AddScoped<IDbInitializer, EFInMemoryDbInitializer>();
        }
    }
}
