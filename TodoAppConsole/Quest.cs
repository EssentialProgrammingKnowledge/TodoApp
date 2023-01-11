namespace TodoAppConsole
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public QuestStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Title: {Title} Status: {Status} Created: {Created} Modified: {Modified} \nDescription:{Description}";
        }
    }
}
