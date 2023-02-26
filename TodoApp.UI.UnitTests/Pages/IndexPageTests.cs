using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using TodoApp.Shared.DTO;
using TodoApp.UI.Pages;
using TodoApp.UI.Services;
using TodoApp.UI.UnitTests.Common;

namespace TodoApp.UI.UnitTests.Pages
{
    public class IndexPageTests
    {
        [Fact]
        public void should_render_index_page()
        {
            _component.ShouldNotBeNull();
            var infromationText = _component.Find("[data-name=\"quests-information\"]");
            infromationText.ShouldNotBeNull();
            infromationText.TextContent.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task given_quests_should_render_page_with_filled_table()
        {
            var quests = TestFixture.GetQuests().ToList();
            var component = await CreateComponentWithQuests(quests);

            var table = component.Find(".table.table-striped");
            table.ShouldNotBeNull();
            quests.ForEach(q =>
            {
                var element = table.QuerySelector($"[data-name=\"quest-row-action-{q.Id}\"]");
                element.ShouldNotBeNull();
                element.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            });
        }

        [Fact]
        public void when_fetching_data_should_show_spinner()
        {
            _component.Instance.Loading = true;

            var spinner = _component.Find("[data-name=\"spinner\"]");
            spinner.ShouldNotBeNull();
            spinner.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            spinner.InnerHtml.ShouldContain("Loading ...");
        }

        [Fact]
        public void should_find_add_button()
        {
            RenderComponentWithoutLoadingIcon();

            var button = _component.Find("[data-name=\"quest-add-button\"]");

            button.ShouldNotBeNull();
            button.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            button.TextContent.ShouldContain("Add");
        }

        [Fact]
        public void should_add_quest()
        {
            RenderComponentWithoutLoadingIcon();
            var button = _component.Find("[data-name=\"quest-add-button\"]");
            button.Click();
            var titleInput = _component.Find("[data-name=\"quest-title-input\"]");
            var titleString = "Title#20";
            titleInput.Change(titleString);
            var sendButton = _component.Find("[data-name=\"quest-submit-button\"]");

            sendButton.Click();

            _questService.Verify(q => q.Add(It.Is<QuestDto>(p => p.Title == titleString)), times: Times.Once);
        }

        [Fact]
        public async Task should_update_quest()
        {
            var quests = TestFixture.GetQuests().ToList();
            var quest = quests.First();
            var component = await CreateComponentWithQuests(quests);
            var button = component.Find($"[data-name=\"quest-{quest.Id}-edit-action\"]");
            button.Click();
            var titleInput = component.Find("[data-name=\"quest-title-input\"]");
            var titleString = "Title#20";
            titleInput.Change(titleString);
            var statusSelect = component.Find("[data-name=\"quest-status-input\"]");
            var status = "Complete";
            statusSelect.Change(status);
            var sendButton = component.Find("[data-name=\"quest-submit-button\"]");

            sendButton.Click();

            _questService.Verify(q => q.Update(It.Is<QuestDto>(p => p.Title == titleString && p.Status == status)), times: Times.Once);
        }

        [Fact]
        public async Task should_delete_quest()
        {
            var quests = TestFixture.GetQuests().ToList();
            var quest = quests.First();
            var component = await CreateComponentWithQuests(quests);
            var button = component.Find($"[data-name=\"quest-{quest.Id}-delete-action\"]");
            button.Click();
            var confirmButton = component.Find("[data-name=\"quest-delete-action-confirm\"]");

            confirmButton.Click();

            _questService.Verify(q => q.Delete(It.Is<int>(id => id == quest.Id)), times: Times.Once);
        }

        [Fact]
        public async Task should_change_quest_status()
        {
            var quests = TestFixture.GetQuests().ToList();
            var quest = quests.First();
            var component = await CreateComponentWithQuests(quests);
            var tableRow = component.Find($"[data-name=\"quest-row-action-{quest.Id}\"]");
            tableRow.Click();
            var completeButton = component.Find("[data-name=\"quest-set-as-complete-button\"]");

            completeButton.Click();

            _questService.Verify(q => q.ChangeStatus(It.Is<ChangeQuestStatus>(p => p.Id == quest.Id)), times: Times.Once);
        }

        private async Task<IRenderedComponent<IndexPage>> CreateComponentWithQuests(List<QuestDto>? questDtos = null)
        {
            var quests = questDtos ?? TestFixture.GetQuests().ToList();
            _questService.Setup(q => q.GetAll()).Returns(quests);
            var component = _testContext.RenderComponent<IndexPage>();
            //delay 1s on init at IndexPage
            await Task.Delay(1200);
            return component;
        }

        private void RenderComponentWithoutLoadingIcon()
        {
            _component.Instance.Loading = false;
            _component.Render();
        }

        private readonly Mock<IQuestService> _questService;
        private readonly IRenderedComponent<IndexPage> _component;
        private readonly TestContext _testContext;

        public IndexPageTests()
        {
            _testContext = new TestContext();
            _questService = new Mock<IQuestService>();
            _testContext.Services.AddScoped(_ => _questService.Object);
            _component = _testContext.RenderComponent<IndexPage>();
        }
    }
}
