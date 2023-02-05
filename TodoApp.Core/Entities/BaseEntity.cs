namespace TodoApp.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; }

        public BaseEntity(int id)
        {
            Id = id;
        }
    }
}
