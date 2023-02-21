using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.IntegrationTests.Common
{
    [Collection("integration-todo-testing")]
    public abstract class BaseTest : IDisposable
    {
        protected HttpClient Client;
        private readonly TestApplicationFactory Fixture;
        private IServiceScope? Scope;

        public BaseTest(TestApplicationFactory testApplicationFactory)
        {
            Fixture = testApplicationFactory;
            Client = testApplicationFactory.CreateClient();
        }

        protected T? GetService<T>()
        {
            Scope ??= Fixture.Services.CreateScope();
            return Scope.ServiceProvider.GetService<T>();
        }

        protected T GetRequiredService<T>() where T : notnull
        {
            Scope ??= Fixture.Services.CreateScope();
            return Scope.ServiceProvider.GetRequiredService<T>();
        }

        public virtual void Dispose()
        {
            Scope?.Dispose();
        }
    }
}
