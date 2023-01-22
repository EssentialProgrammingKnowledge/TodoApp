namespace TodoApp.Core.Entities
{
    public class Menu : BaseEntity
    {
        public string Name { get; set; } = "";

        public override string ToString()
        {
            return $"{Id}. {Name}";
        }
    }
}
