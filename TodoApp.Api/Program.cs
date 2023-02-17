using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using TodoApp.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
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

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    [HttpPost]
    public ActionResult Post(StudentWithFluentValidation student)
    {
        return Ok(student);
    }
}

public class Student
{
    [Required]
    [MinLength(3)]
    [RegularExpression("^[a-zA-Z\\s]*$", ErrorMessage = "Name should contain only letters")]
    public string Name { get; set; } = "";

    [Required]
    [Range(0, 100)]
    public int Age { get; set; }
}

public class StudentWithFluentValidation
{
    public string Name { get; set; } = "";

    public int Age { get; set; }
}

public class StudentWithFluentValidationValidator : AbstractValidator<StudentWithFluentValidation>
{
    public StudentWithFluentValidationValidator()
    {
        RuleFor(s => s.Name).NotNull()
                            .NotEmpty()
                            .MinimumLength(3)
                            .Matches("^[a-zA-Z\\s]*$");
        RuleFor(s => s.Age).GreaterThan(0)
                           .LessThan(100)
                           .NotNull();
    }
}