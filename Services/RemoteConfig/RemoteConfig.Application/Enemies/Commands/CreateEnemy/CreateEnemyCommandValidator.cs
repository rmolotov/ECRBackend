using FluentValidation;

namespace RemoteConfig.Application.Enemies.Commands.CreateEnemy;

public class CreateEnemyCommandValidator : AbstractValidator<CreateEnemyCommand>
{
    public CreateEnemyCommandValidator()
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

        // todo
        // RuleFor(command => command.AttackType)
        //     .IsEnumName();
        // RuleFor(command => command.EnemyType)
        //     .IsEnumName();
    }
}