namespace TodoApp.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public string Name { get; set; } = "";

        public Menu(int id)
            : base(id)
        {
        }

        public override string ToString()
        {
            return $"{Id}. {Name}";
        }
    }
}
