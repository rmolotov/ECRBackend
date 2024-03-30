using MediatR;
using RemoteConfig.Application.Common.Exceptions;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Inventory;

namespace RemoteConfig.Application.InventoryItems.Commands.DeleteInventoryItem;

public class DeleteInventoryItemCommandHandler(IRemoteConfigContext dbContext) : IRequestHandler<DeleteInventoryItemCommand>
{
    public async Task Handle(DeleteInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var inventoryItem = await dbContext.InventoryItems.FindAsync(
            [request.Id],
            cancellationToken);

        if (inventoryItem == null)
            throw new NotFoundException(nameof(InventoryItem), request.Id);

        dbContext.InventoryItems.Remove(inventoryItem);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}