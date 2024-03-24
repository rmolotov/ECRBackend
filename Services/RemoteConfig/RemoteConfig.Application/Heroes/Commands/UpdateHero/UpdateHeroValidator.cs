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
            .NotEqual(0);
        RuleFor(command => command.Capacity)
            .NotEqual(0);
        RuleFor(command => command.Current)
            .NotEqual(0);
        RuleFor(command => command.Voltage)
            .NotEqual(0);
    }
}