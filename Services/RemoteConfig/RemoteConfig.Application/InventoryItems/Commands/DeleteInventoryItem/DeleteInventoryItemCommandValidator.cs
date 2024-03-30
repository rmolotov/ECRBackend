using FluentValidation;

namespace RemoteConfig.Application.InventoryItems.Commands.DeleteInventoryItem;

public class DeleteInventoryItemCommandValidator : AbstractValidator<DeleteInventoryItemCommand>
{
    public DeleteInventoryItemCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .NotEmpty();
    }
}