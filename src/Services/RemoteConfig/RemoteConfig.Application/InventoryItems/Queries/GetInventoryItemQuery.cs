using MediatR;
using RemoteConfig.Application.InventoryItems.Responses;

namespace RemoteConfig.Application.InventoryItems.Queries;

public class GetInventoryItemQuery(string id) : IRequest<GetInventoryItemResponse>
{
    public string Id { get; set; } = id;
}