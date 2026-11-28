using MediatR;
using RemoteConfig.Application.Heroes.Responses;

namespace RemoteConfig.Application.Heroes.Queries;

public class GetHeroQuery(string id) : IRequest<GetHeroResponse>
{
    public string Id { get; set; } = id;
}