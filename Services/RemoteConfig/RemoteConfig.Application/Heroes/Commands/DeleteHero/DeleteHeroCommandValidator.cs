using FluentValidation;

namespace RemoteConfig.Application.Heroes.Commands.DeleteHero;

public class DeleteHeroCommandValidator : AbstractValidator<DeleteHeroCommand>
{
    public DeleteHeroCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotNull();
    }
}