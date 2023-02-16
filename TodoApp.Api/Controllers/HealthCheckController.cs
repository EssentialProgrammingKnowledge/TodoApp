using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "TodoApp!";
        }
    }
}
