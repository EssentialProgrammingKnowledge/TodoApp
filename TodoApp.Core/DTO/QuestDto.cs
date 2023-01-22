using TodoApp.Core.Entities;

namespace TodoApp.Core.DTO
{
    public class QuestDto : IBaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = QuestStatus.New.ToString();
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Title: {Title} Status: {Status} Created: {Created} Modified: {Modified} \nDescription:{Description}";
        }
    }
}
