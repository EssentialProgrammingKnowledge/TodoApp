using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
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
builder.Services.AddSingleton<ErrorHandlerMiddleware>();
builder.Services.AddHostedService<HostedServiceTest>();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<InternalErrorExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.MapGet("/api/hc", (IOptionsMonitor<AppOptions> options) => options.CurrentValue);
app.MapPost("/api/hc", (int id) => Results.Ok($"Resource Added {id}"));
app.MapGet("/api/bad-request", () => { throw new InvalidOperationException("Should return BadRequest 400 status"); });
app.MapGet("/api/internal-error", () => { throw new Exception("Should return InternalErrorServer 500 status"); });

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

    [HttpGet]
    public ActionResult Get()
    {
        throw new NotImplementedException();
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

class BadRequestExceptionHandler : IExceptionHandler
{
    private readonly ILogger<BadRequestExceptionHandler> _logger;

    public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidOperationException badRequestException)
        {
            return false;
        }

        _logger.LogError(
            badRequestException,
            "Exception occurred: {Message}",
            badRequestException.Message);

        var problemDetails = new
        {
            Status = StatusCodes.Status400BadRequest,
            Detail = badRequestException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status;

        await httpContext.Response
            .WriteAsJsonAsync(new Dictionary<string, string>() { { "Error", problemDetails.Detail } }, cancellationToken);

        return true;
    }
}

class InternalErrorExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InternalErrorExceptionHandler> _logger;

    public InternalErrorExceptionHandler(ILogger<InternalErrorExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error"
        };

        httpContext.Response.StatusCode = problemDetails.Status;
        await httpContext.Response
            .WriteAsJsonAsync(new Dictionary<string, string>() { { "Error", problemDetails.Title } }, cancellationToken);

        return true;
    }
}
