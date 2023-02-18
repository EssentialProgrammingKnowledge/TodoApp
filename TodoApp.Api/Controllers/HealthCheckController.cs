using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoApp.Infrastructure;

namespace TodoApp.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly AppOptions _appOptions;

        public HealthCheckController(IOptionsMonitor<AppOptions> options)
        {
            _appOptions = options.CurrentValue;
        }

        [HttpGet]
        public string? Get()
        {
            return _appOptions.Name;
        }
    }
}
