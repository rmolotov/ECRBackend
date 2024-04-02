using FluentValidation;
using RemoteConfig.Core.Entities.Enemy;

namespace RemoteConfig.Application.Enemies.Commands.CreateEnemy;

public class CreateEnemyCommandValidator : AbstractValidator<CreateEnemyCommand>
{
    public CreateEnemyCommandValidator()
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
        
        RuleFor(command => command.AttackType)
            .IsEnumName(typeof(AttackType), caseSensitive: false);
        RuleFor(command => command.EnemyType)
            .IsEnumName(typeof(EnemyType), caseSensitive: false);
    }
}