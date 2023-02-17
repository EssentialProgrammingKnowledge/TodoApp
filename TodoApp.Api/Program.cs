using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Options;
using TodoApp.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("app"));

builder.Services.AddHostedService<HostedServiceTest>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapGet("/api/hc", (IOptionsMonitor<AppOptions> options) => options.CurrentValue);
app.MapPost("/api/hc", (int id) => Results.Ok($"Resource Added {id}"));

app.Run();

class HostedServiceTest : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}