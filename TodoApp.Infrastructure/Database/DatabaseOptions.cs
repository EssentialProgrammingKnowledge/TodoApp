namespace TodoApp.Infrastructure.Database
{
    public class DatabaseOptions
    {
        public string? ConnectionString { get; set; }
        public bool AllowMigrations { get; set; } = false;
    }
}
