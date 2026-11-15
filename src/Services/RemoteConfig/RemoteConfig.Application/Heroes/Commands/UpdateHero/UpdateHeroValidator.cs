using FluentValidation;

namespace RemoteConfig.Application.Heroes.Commands.UpdateHero;

public class UpdateHeroValidator : AbstractValidator<UpdateHeroCommand>
{
    public UpdateHeroValidator()
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