using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Inventory;

namespace RemoteConfig.Application.InventoryItems.Commands.UpdateInventoryItem;

public class UpdateInventoryItemCommandHandler(IRemoteConfigContext dbContext, IMapper mapper) 
    : IRequestHandler<UpdateInventoryItemCommand>
{
    public async Task Handle(UpdateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.InventoryItems.FirstOrDefaultAsync(
            h => h.Id == request.Id, cancellationToken);
        
        if (entity == null)
            throw new NotFoundException(nameof(InventoryItem), request.Id);

        entity.Title       = request.Title;
        entity.Description = request.Description;
        entity.MaxCount    = request.MaxCount;
        entity.Price       = request.Price;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}