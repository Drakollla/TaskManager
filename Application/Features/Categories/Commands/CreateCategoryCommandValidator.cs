using FluentValidation;

namespace Application.Features.Categories.Commands
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.CreateCategoryDto.Name)
                .NotEmpty().WithMessage("Category name cannot be empty.")
                .MaximumLength(50).WithMessage("The name is too long (max 50 characters)");
        }
    }
}