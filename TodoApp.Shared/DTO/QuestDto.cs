using FluentValidation;

namespace TodoApp.Shared.DTO
{
    public class QuestDto : IBaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "New";
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Title: {Title} Status: {Status} Created: {Created} Modified: {Modified} \nDescription:{Description}";
        }
    }

    public class QuestDtoValidator : AbstractValidator<QuestDto>
    {
        public QuestDtoValidator()
        {
            RuleFor(q => q.Title).MinimumLength(2);
            RuleFor(q => q.Status).NotNull()
                                  .NotEmpty();

            When(q => !string.IsNullOrWhiteSpace(q.Description), () =>
            {
                RuleFor(quest => quest.Description).MaximumLength(3000);
            });
        }
    }
}
