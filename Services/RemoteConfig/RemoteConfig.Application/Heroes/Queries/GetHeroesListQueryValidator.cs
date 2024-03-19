using FluentValidation;

namespace RemoteConfig.Application.Heroes.Queries;

public class GetHeroesListQueryValidator : AbstractValidator<GetHeroesListQuery>
{
    public GetHeroesListQueryValidator()
    {
        
    }
}