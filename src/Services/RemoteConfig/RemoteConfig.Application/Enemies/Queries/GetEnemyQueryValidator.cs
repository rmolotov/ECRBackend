using FluentValidation;

namespace RemoteConfig.Application.Enemies.Queries;

public class GetEnemyQueryValidator : AbstractValidator<GetEnemyQuery>
{
    public GetEnemyQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .NotNull();
    }
}