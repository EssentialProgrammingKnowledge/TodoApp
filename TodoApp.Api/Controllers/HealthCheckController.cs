using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TodoApp.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppOptions _appOptions;

        public HealthCheckController(IConfiguration configuration, IOptionsMonitor<AppOptions> options)
        {
            _configuration = configuration;
            _appOptions = options.CurrentValue;
        }

        [HttpGet]
        public string? Get()
        {
            //return _configuration.GetRequiredSection("app").Value;
            return _appOptions.Name;
        }
    }
}
