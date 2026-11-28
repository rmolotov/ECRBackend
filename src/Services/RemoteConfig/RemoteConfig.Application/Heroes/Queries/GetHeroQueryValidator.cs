using FluentValidation;

namespace RemoteConfig.Application.Heroes.Queries;

public class GetHeroQueryValidator : AbstractValidator<GetHeroQuery>
{
    public GetHeroQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}
