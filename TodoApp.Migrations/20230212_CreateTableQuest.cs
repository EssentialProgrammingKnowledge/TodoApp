using FluentMigrator;

namespace TodoApp.Migrations
{
    [Migration(20230212162015)]
    public class CreateTableQuest : Migration
    {
        private readonly string _location = Path.GetDirectoryName(typeof(CreateTableQuest).Assembly.Location)!;

        public override void Down()
        {
            var scriptPath = Path.Combine(_location!, "delete_quest_table.sql");
            Execute.Script(scriptPath);
        }

        public override void Up()
        {
            var scriptPath = Path.Combine(_location!, "create_quest_table.sql");
            Execute.Script(scriptPath);
        }
    }
}
