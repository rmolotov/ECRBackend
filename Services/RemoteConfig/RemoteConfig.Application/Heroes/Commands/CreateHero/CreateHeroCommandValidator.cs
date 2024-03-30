using FluentValidation;

namespace RemoteConfig.Application.Heroes.Commands.CreateHero;

public class CreateHeroCommandValidator : AbstractValidator<CreateHeroCommand>
{
    public CreateHeroCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .MaximumLength(64);
        RuleFor(command => command.Resistance)
            .GreaterThan(0);
        RuleFor(command => command.Capacity)
            .GreaterThan(0);
        RuleFor(command => command.Current)
            .GreaterThan(0);
        RuleFor(command => command.Voltage)
            .GreaterThan(0);
    }
}