using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Application.InventoryItems.Responses;

namespace RemoteConfig.Application.InventoryItems.Queries;

public class GetInventoryItemsListQueryHandler(IRemoteConfigContext dbContext, IMapper mapper)
    : IRequestHandler<GetInventoryItemsListQuery, IList<GetInventoryItemResponse>> 
{
    public async Task<IList<GetInventoryItemResponse>> Handle(GetInventoryItemsListQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.InventoryItems
            .ToListAsync(cancellationToken);
        
        return mapper.Map<IList<GetInventoryItemResponse>>(list);
    }
}