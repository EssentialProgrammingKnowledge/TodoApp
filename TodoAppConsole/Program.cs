using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using TodoApp.Core;
using TodoApp.Infrastructure;
using TodoAppConsole;

IServiceCollection Setup(string connectionString)
{
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddCore()
                     .AddInfrastructure(connectionString)
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

async Task<string> GetDbConnection()
{
    var appFolder = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName 
        ?? throw new InvalidOperationException("Cannot get Application folder directory");
    using FileStream fileStream = File.Open(appFolder + Path.DirectorySeparatorChar + "appsettings.json", FileMode.Open, FileAccess.Read);
    var settings = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(fileStream) 
            ?? throw new InvalidOperationException("Cannot deserialize appsettings please ensure that this file exists");
    var connectionString = settings["database"]?.ToString();

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("There is no connection string in 'appsettings.json'. Please fill the database section");
    }
    return connectionString;
}

var connectionString = await GetDbConnection();
var serviceCollection = Setup(connectionString);
var serviceProvider = serviceCollection.BuildServiceProvider();
serviceProvider.UseInfrastructure();
var todoInteractionService = serviceProvider.GetRequiredService<ITodoInteractionService>();
await todoInteractionService.Start();
