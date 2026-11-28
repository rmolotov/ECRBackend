using MediatR;
using RemoteConfig.Application.InventoryItems.Responses;

namespace RemoteConfig.Application.InventoryItems.Queries;

public class GetInventoryItemsListQuery : IRequest<IList<GetInventoryItemResponse>>
{
    
}