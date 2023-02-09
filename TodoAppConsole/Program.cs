using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoApp.Core;
using TodoApp.Infrastructure;
using TodoAppConsole;

IServiceCollection Setup()
{
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddCore()
                     .AddInfrastructure()
                     .AddSingleton<ITodoInteractionService, TodoInteractionService>();
    Assembly.GetExecutingAssembly().GetTypes()
        .AsParallel()
        .Where(t => typeof(IConsoleView).IsAssignableFrom(t) && t != typeof(IConsoleView))
        .ToList()
        .ForEach(t =>
        {
            serviceCollection.AddScoped(t);
        });
    return serviceCollection;
}


var serviceCollection = Setup();
var serviceProvider = serviceCollection.BuildServiceProvider();
serviceProvider.UseInfrastructure();
var todoInteractionService = serviceProvider.GetRequiredService<ITodoInteractionService>();
await todoInteractionService.Start();
