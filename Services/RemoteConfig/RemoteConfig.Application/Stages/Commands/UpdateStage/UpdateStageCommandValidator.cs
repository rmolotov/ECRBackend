using FluentValidation;

namespace RemoteConfig.Application.Stages.Commands.UpdateStage;

public class UpdateStageCommandValidator : AbstractValidator<UpdateStageCommand>
{
    public UpdateStageCommandValidator()
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