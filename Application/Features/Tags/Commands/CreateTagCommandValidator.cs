using FluentValidation;

namespace Application.Features.Tags.Commands
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.CreateTagDto.Name)
                .Empty()
                .MaximumLength(50);

            RuleFor(x => x.CreateTagDto.ColorHex)
                .NotEmpty()
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                .WithMessage("Color must be format HEX (for example,  #FF0000)");
        }
    }
}