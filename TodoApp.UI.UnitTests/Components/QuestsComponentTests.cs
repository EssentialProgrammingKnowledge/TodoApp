using Bunit;
using Shouldly;
using TodoApp.Shared.DTO;
using TodoApp.UI.Components;
using TodoApp.UI.UnitTests.Common;

namespace TodoApp.UI.UnitTests.Components
{
    public class QuestsComponentTests
    {
        [Fact]
        public void should_render_component()
        {
            _component.Markup.ShouldContain("Id");
            var table = _component.Find(".table.table-striped");
            table.ShouldNotBeNull();
            table.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void given_quests_should_render_component_with_filled_table()
        {
            var quests = TestFixture.GetQuests().ToList();
            
            _component.SetParametersAndRender(parameters => parameters.Add(param => param.Quests, quests));

            var table = _component.Find(".table.table-striped");
            table.ShouldNotBeNull();
            quests.ForEach(q =>
            {
                var element = table.QuerySelector($"[data-name=\"quest-row-action-{q.Id}\"]");
                element.ShouldNotBeNull();
                element.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            });
        }

        [Fact]
        public void given_quests_when_clicked_edit_button_should_show_modal_edit_quest()
        {
            var quests = TestFixture.GetQuests().ToList();
            var quest = quests.First();
            _component.SetParametersAndRender(parameters => parameters.Add(param => param.Quests, quests));
            var modalEdit = _component.Find($"[data-name=\"quest-{quest.Id}-edit-action\"]");

            modalEdit.Click();

            var modalTitle = _component.Find("[data-name=\"modal-title\"]");
            modalTitle.ShouldNotBeNull();
            modalTitle.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            modalTitle.TextContent.ShouldBe("Edit Quest");
        }

        [Fact]
        public void given_quests_and_invalid_title_when_edit_quest_should_show_validation_error()
        {
            var quests = TestFixture.GetQuests().ToList();
            var quest = quests.First();
            _component.SetParametersAndRender(parameters => parameters.Add(param => param.Quests, quests));
            var modalEdit = _component.Find($"[data-name=\"quest-{quest.Id}-edit-action\"]");
            modalEdit.Click();
            var titleInput = _component.Find("[data-name=\"quest-title-input\"]");

            titleInput.Change("");

            var titleValidation = _component.Find("[data-name=\"quest-title-validation\"]");
            titleValidation.ShouldNotBeNull();
            titleValidation.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void given_quests_when_clicked_delete_button_should_show_modal_delete_quest()
        {
            var quests = TestFixture.GetQuests().ToList();
            var quest = quests.First();
            _component.SetParametersAndRender(parameters => parameters.Add(param => param.Quests, quests));
            var modalDelete = _component.Find($"[data-name=\"quest-{quest.Id}-delete-action\"]");

            modalDelete.Click();

            var modalTitle = _component.Find("[data-name=\"modal-title\"]");
            modalTitle.ShouldNotBeNull();
            modalTitle.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            modalTitle.TextContent.ShouldBe("Delete Quest");
        }

        [Fact]
        public void given_quests_when_change_quest_status_should_send_event_callback_on_change_status()
        {
            ChangeQuestStatus? changeQuestStatus = null;
            var quests = TestFixture.GetQuests().ToList();
            var quest = quests.First();
            _component.SetParametersAndRender(parameters => {
                parameters.Add(param => param.Quests, quests);
                parameters.Add(param => param.OnChangeStatus, (s) => changeQuestStatus = s);
            });
            var firstTd = _component.Find($"[data-name=\"quest-row-action-{quest.Id}\"] > td");
            firstTd.Click();
            var setAsCompleteButton = _component.Find("[data-name=\"quest-set-as-complete-button\"]");
            _component.Instance.QuestClicked.ShouldNotBeNull();

            setAsCompleteButton.Click();

            // po zmianie statusu chowaj element
            _component.Instance.QuestClicked.ShouldBeNull();
            changeQuestStatus.ShouldNotBeNull();
            changeQuestStatus.Id.ShouldBe(quest.Id);
        }

        private readonly IRenderedComponent<QuestsComponent> _component;

        public QuestsComponentTests()
        {
            var testContext = new TestContext();
            _component = testContext.RenderComponent<QuestsComponent>();
        }
    }
}
