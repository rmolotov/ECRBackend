using MediatR;

namespace RemoteConfig.Application.InventoryItems.Commands.DeleteInventoryItem;

public class DeleteInventoryItemCommand : IRequest
{
    public string Id { get; set; }
}