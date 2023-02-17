using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HealthCheckController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string? Get()
        {
            return _configuration.GetRequiredSection("app").Value;
        }
    }
}
