using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Application.InventoryItems.Responses;
using RemoteConfig.Core.Entities.Inventory;

namespace RemoteConfig.Application.InventoryItems.Queries;

public class GetInventoryItemQueryHandler(IRemoteConfigContext dbContext, IMapper mapper)
    : IRequestHandler<GetInventoryItemQuery, GetInventoryItemResponse>
{
    public async Task<GetInventoryItemResponse> Handle(GetInventoryItemQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.InventoryItems.FirstOrDefaultAsync(
            e => e.Id == request.Id, 
            cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(InventoryItem), request.Id);

        return mapper.Map<GetInventoryItemResponse>(entity);
    }
}