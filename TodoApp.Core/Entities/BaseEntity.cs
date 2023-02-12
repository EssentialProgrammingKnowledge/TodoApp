using System.Text.Json.Serialization;

namespace TodoApp.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; private set; }

        public BaseEntity(int id)
        {
            Id = id;
        }

        protected BaseEntity() { }
    }
}
