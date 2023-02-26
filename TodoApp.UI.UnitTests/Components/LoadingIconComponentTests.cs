using Bunit;
using Shouldly;
using TodoApp.UI.Components;

namespace TodoApp.UI.UnitTests.Components
{
    public class LoadingIconComponentTests
    {
        [Fact]
        public void should_render_component()
        {
            var component = _testContext.RenderComponent<LoadingIconComponent>();

            component.ShouldNotBeNull();
            component.Instance.ShouldNotBeNull();
            var spinner = component.Find("[data-name=\"spinner\"]");
            spinner.ShouldNotBeNull();
            spinner.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            spinner.InnerHtml.ShouldContain("Loading ...");
        }

        private readonly TestContext _testContext;

        public LoadingIconComponentTests()
        {
            _testContext = new TestContext();
        }
    }
}