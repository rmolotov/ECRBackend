using FluentValidation;

namespace RemoteConfig.Application.Stages.Queries;

public class GetStageQueryValidator : AbstractValidator<GetStageQuery>
{
    public GetStageQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .NotNull();
    }
}