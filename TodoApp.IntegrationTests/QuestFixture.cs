using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core;
using TodoApp.Infrastructure;

namespace TodoApp.IntegrationTests
{
    public class QuestFixture
    {
        public IServiceProvider ServiceProvider { get; set; }

        public QuestFixture()
        {
            ServiceProvider = new ServiceCollection().AddCore()
                                                     .AddInfrastructure("Host=localhost;Database=todoappdatabase;Username=root;Password=")
                                                     .BuildServiceProvider();
        }
    }
}
