using FluentValidation;

namespace RemoteConfig.Application.InventoryItems.Commands.UpdateInventoryItem;

public class UpdateInventoryItemCommandValidator : AbstractValidator<UpdateInventoryItemCommand>
{
    public UpdateInventoryItemCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .MaximumLength(64);
        RuleFor(command => command.Title)
            .NotNull()
            .NotEmpty();
        RuleFor(command => command.Description)
            .NotNull()
            .NotEmpty();
        RuleFor(command => command.MaxCount)
            .GreaterThan(0);
        RuleFor(command => command.Price)
            .GreaterThan(0);
    }
}