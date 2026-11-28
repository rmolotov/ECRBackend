using FluentValidation;

namespace RemoteConfig.Application.InventoryItems.Queries;

public class GetInventoryItemQueryValidator : AbstractValidator<GetInventoryItemQuery>
{
    public GetInventoryItemQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotNull()
            .NotEmpty();
    }
}