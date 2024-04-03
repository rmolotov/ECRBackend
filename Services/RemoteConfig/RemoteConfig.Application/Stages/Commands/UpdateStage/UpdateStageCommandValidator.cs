using FluentValidation;
using RemoteConfig.Core.Entities.Stage;

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

        RuleForEach(command => command.BoardTiles)
            .NotNull()
            .Must(tile => tile.Length == 4)
            .ChildRules(tile =>
            {
                tile.RuleFor(t => (BoardTileType)t[0]).IsInEnum();
                tile.RuleFor(t => (BoardTileRotation)t[1]).IsInEnum();
            });

        RuleFor(command => command.EnemySpawners)
            .NotNull();
    }
}