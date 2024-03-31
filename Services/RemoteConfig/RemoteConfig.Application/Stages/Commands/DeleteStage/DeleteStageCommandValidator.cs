using FluentValidation;

namespace RemoteConfig.Application.Stages.Commands.DeleteStage;

public class DeleteStageCommandValidator : AbstractValidator<DeleteStageCommand>
{
    public DeleteStageCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .NotEmpty();
    }
}