using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using TodoApp.Core;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Database;

namespace TodoAppConsole;

class Program : IDesignTimeDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext(string[] args)
    {
        var appsettings = GetAppsettings().GetAwaiter().GetResult();
        var serviceCollection = Setup(appsettings);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider.GetRequiredService<TodoDbContext>();
    }

    public static async Task Main(string[] args)
    {
        var appsettings = await GetAppsettings();
        var serviceCollection = Setup(appsettings);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        serviceProvider.UseInfrastructure();
        var todoInteractionService = serviceProvider.GetRequiredService<ITodoInteractionService>();
        await todoInteractionService.Start();
    }

    private static IServiceCollection Setup(Dictionary<string, object> appsettings)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddCore()
                         .AddInfrastructure(appsettings)
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

    private static async Task<Dictionary<string, object>> GetAppsettings()
    {
        var appFolder = AppContext.BaseDirectory
            ?? throw new InvalidOperationException("Cannot get Application folder directory");
        using FileStream fileStream = File.Open(appFolder + Path.DirectorySeparatorChar + "appsettings.json", FileMode.Open, FileAccess.Read);
        var settings = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(fileStream)
                ?? throw new InvalidOperationException("Cannot deserialize 'appsettings.json', please ensure that this file exists");
        return settings;
    }
}