using FluentMigrator;

namespace TodoApp.Migrations
{
    [Migration(20230212162015)]
    public class CreateTableQuest : Migration
    {
        public override void Down()
        {
            Execute.Script("delete_quest_table.sql");
        }

        public override void Up()
        {
            Execute.Script("create_quest_table.sql");
        }
    }
}
