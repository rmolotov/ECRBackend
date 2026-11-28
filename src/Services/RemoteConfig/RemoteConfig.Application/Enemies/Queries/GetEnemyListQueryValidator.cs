using FluentValidation;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemyListQueryValidator : AbstractValidator<GetEnemiesListQuery>
{
    public GetEnemyListQueryValidator()
    {
        
    }
}