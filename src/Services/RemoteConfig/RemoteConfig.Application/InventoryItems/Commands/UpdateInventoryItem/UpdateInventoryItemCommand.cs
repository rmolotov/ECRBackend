using MediatR;

namespace RemoteConfig.Application.InventoryItems.Commands.UpdateInventoryItem;

public class UpdateInventoryItemCommand : IRequest
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxCount { get; set; }
    public int Price { get; set; }
}