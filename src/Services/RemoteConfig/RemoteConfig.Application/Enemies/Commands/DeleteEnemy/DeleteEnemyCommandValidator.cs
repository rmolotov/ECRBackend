using FluentValidation;

namespace RemoteConfig.Application.Enemies.Commands.DeleteEnemy;

public class DeleteEnemyCommandValidator : AbstractValidator<DeleteEnemyCommand>
{
    public DeleteEnemyCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .NotEmpty();
    }
}