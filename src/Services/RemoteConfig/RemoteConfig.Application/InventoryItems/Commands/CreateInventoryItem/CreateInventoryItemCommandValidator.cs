using FluentValidation;

namespace RemoteConfig.Application.InventoryItems.Commands.CreateInventoryItem;

public class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
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