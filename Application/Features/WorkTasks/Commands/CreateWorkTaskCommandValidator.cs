using FluentValidation;

namespace Application.Features.WorkTasks.Commands
{
    public class CreateWorkTaskCommandValidator : AbstractValidator<CreateWorkTaskCommand>
    {
        public CreateWorkTaskCommandValidator()
        {
            RuleFor(x => x.TaskDto.Title).NotEmpty();
            RuleFor(x => x.TaskDto.DueDate).GreaterThan(DateTime.Now);
        }
    }
}