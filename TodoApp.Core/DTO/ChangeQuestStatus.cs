using FluentValidation;

namespace TodoApp.Core.DTO
{
    public record ChangeQuestStatus(string Status);

    public class ChangeQuestStatusValidator : AbstractValidator<ChangeQuestStatus>
    {
        public ChangeQuestStatusValidator()
        {
            RuleFor(c => c.Status).NotNull()
                                  .NotEmpty();
        }
    }
}
