using FluentValidation;

namespace TodoApp.Shared.DTO
{
    public record ChangeQuestStatus(int Id, string Status);

    public class ChangeQuestStatusValidator : AbstractValidator<ChangeQuestStatus>
    {
        public ChangeQuestStatusValidator()
        {
            RuleFor(c => c.Status).NotNull()
                                  .NotEmpty();
        }
    }
}
