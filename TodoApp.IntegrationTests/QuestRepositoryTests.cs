using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.Entities;
using TodoApp.Core.Repositories;

namespace TodoApp.IntegrationTests
{
    public class QuestRepositoryTests : IClassFixture<QuestFixture>
    {
        [Fact]
        public async Task should_add_quest_to_database()
        {
            var quest = Quest.Create("Quest#1", "Description123");

            var id = await _repository.Add(quest);

            var questAdded = await _repository.Get(id);
            Assert.NotNull(questAdded);
            Assert.True(questAdded.Title == quest.Title);
            Assert.True(questAdded.Description == quest.Description);
        }

        [Fact]
        public async Task should_update_quest()
        {
            var quest = Quest.Create("Quest#2", "Description123");
            var id = await _repository.Add(quest);
            var questAdded = new Quest(id, quest.Title, quest.Description, quest.Status, quest.Created, quest.Modified);
            questAdded.ChangeStatus("Complete");
            questAdded.ChangeTitle("Title#123");
            questAdded.ChangeDescription(null);

            await _repository.Update(questAdded);

            var questUpdated = await _repository.Get(id);
            Assert.NotNull(questUpdated);
            Assert.True(questAdded.Status == questUpdated.Status);
            Assert.True(questAdded.Title == questUpdated.Title);
            Assert.True(questAdded.Description == questUpdated.Description);
        }

        [Fact]
        public async Task should_delete_quest()
        {
            var quest = Quest.Create("Quest#3", "Description123");
            var id = await _repository.Add(quest);

            await _repository.Delete(new Quest(id, quest.Title, quest.Description, quest.Status, quest.Created, quest.Modified));

            var questDeleted = await _repository.Get(id);
            Assert.Null(questDeleted);
        }

        [Fact]
        public async Task should_get_all_quests()
        {
            await _repository.Add(Quest.Create("Quest#4", "Description123"));
            await _repository.Add(Quest.Create("Quest#5", "Description123"));

            var quests = await _repository.GetAll();

            Assert.NotNull(quests);
            Assert.NotEmpty(quests);
            Assert.True(quests.Count > 1);
        }

        private readonly IRepository<Quest> _repository;

        public QuestRepositoryTests(QuestFixture questFixture)
        {
            using var scope = questFixture.ServiceProvider.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IRepository<Quest>>();
        }
    }
}