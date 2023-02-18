using TodoApp.Domain.Exceptions;

namespace TodoApp.Domain.Entities
{
    public class Quest : BaseEntity
    {
        public string Title { get; private set; } = "";
        public string Description { get; private set; } = "";
        public QuestStatus Status { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Modified { get; private set; }

        private Quest() : base() { }

        public Quest(int id, string title, string description, string status, DateTime created, DateTime? modified = null)
            : base(id)
        {
            ChangeTitle(title);
            ChangeDescription(description);
            ChangeStatus(status);
            Created = created;
            Modified = modified;
        }

        public Quest(int id, string title, string description, QuestStatus status, DateTime created, DateTime? modified = null)
            : base(id)
        {
            ChangeTitle(title);
            ChangeDescription(description);
            Status = status;
            Created = created;
            Modified = modified;
        }

        public static Quest Create(string title, string description)
        {
            return new Quest(0, title, description, QuestStatus.New, DateTime.UtcNow);
        }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) 
            {
                throw new CustomException("Title cannot be empty");
            }

            if (title.Length < 2)
            {
                throw new CustomException("Title should contain at least 2 characters");
            }

            Title = title;
            Modified = DateTime.UtcNow;
        }

        public void ChangeDescription(string description)
        {
            Description = description;
            Modified = DateTime.UtcNow;
        }

        public void ChangeStatus(string status)
        {
            var parsed = Enum.TryParse<QuestStatus>(status, out var statusParsed);

            if (!parsed)
            {
                throw new CustomException($"There is no Quest status {status}");
            }

            if (!Enum.IsDefined(statusParsed))
            {
                throw new CustomException($"There is no Quest status {status}");
            }

            Status = statusParsed;
            Modified = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Id: {Id} Title: {Title} Status: {Status} Created: {Created} Modified: {Modified} \nDescription:{Description}";
        }
    }
}
