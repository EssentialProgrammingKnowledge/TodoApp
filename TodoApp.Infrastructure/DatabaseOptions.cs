namespace TodoApp.Infrastructure
{
    public class DatabaseOptions
    {
        public string? ConnectionString { get; set; }
        public bool AllowMigrations { get; set; }
    }
}
