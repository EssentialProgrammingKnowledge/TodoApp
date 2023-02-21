using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shouldly;
using TodoApp.Infrastructure;
using TodoApp.IntegrationTests.Common;

namespace TodoApp.IntegrationTests.Controllers
{
    public class HealthCheckControllerTests : BaseTest
    {
        [Fact]
        public async Task should_get_health_check_status()
        {
            var options = GetRequiredService<IOptions<AppOptions>>();

            var response = await Client.GetAsync("api");
            
            ((int)response.StatusCode).ShouldBe(StatusCodes.Status200OK);
            var healthCheckStatus = await response.Content.ReadAsStringAsync();
            healthCheckStatus.ShouldBe(options.Value.Name);
        }

        public HealthCheckControllerTests(TestApplicationFactory testApplicationFactory) : base(testApplicationFactory)
        {
        }
    }
}
