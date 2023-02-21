namespace TodoApp.IntegrationTests.Common
{
    [CollectionDefinition("integration-todo-testing")]
    public class SharedClassFixture : IClassFixture<TestApplicationFactory>
    {
    }
}
