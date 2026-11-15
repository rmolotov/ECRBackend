using MediatR;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Core.Entities.Inventory;

namespace RemoteConfig.Application.InventoryItems.Commands.CreateInventoryItem;

public class CreateInventoryItemCommandHandler(IRemoteConfigContext dbContext)
    : IRequestHandler<CreateInventoryItemCommand, string>
{
    public async Task<string> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var inventoryItem = new InventoryItem()
        {
            Id = request.Id,
            
            Title = request.Title,
            Description = request.Description,
            
            MaxCount = request.MaxCount,
            Price = request.Price
        };

        await dbContext.InventoryItems.AddAsync(inventoryItem, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return inventoryItem.Id;
    }
}