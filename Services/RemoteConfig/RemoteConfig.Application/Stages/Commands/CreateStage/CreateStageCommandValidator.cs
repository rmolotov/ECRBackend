using FluentValidation;

namespace RemoteConfig.Application.Stages.Commands.CreateStage;

public class CreateStageCommandValidator : AbstractValidator<CreateStageCommand>
{
    public CreateStageCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .MaximumLength(64);
        RuleFor(command => command.StageTitle)
            .NotNull()
            .NotEmpty()
            .MaximumLength(128);
        RuleFor(command => command.StageDescription)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(command => command.PlayerSpawnPoint)
            .Must(sp => sp.Count == 3);

        RuleFor(command => command.BoardTiles)
            .NotNull();

        RuleFor(command => command.EnemySpawners)
            .NotNull();
    }
}