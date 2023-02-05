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
                     .AddSingleton<IQuestIteractionService, QuestIteractionService>();
    Assembly.GetExecutingAssembly().GetTypes()
        .AsParallel()
        .Where(t => typeof(IConsoleView).IsAssignableFrom(t) && t != typeof(IConsoleView))
        .ToList()
        .ForEach(t =>
        {
            serviceCollection.AddSingleton(t);
        });
    return serviceCollection;
}

var serviceCollection = Setup();
var serviceProvider = serviceCollection.BuildServiceProvider();
var questInteractionService = serviceProvider.GetRequiredService<IQuestIteractionService>();
questInteractionService.Start();
