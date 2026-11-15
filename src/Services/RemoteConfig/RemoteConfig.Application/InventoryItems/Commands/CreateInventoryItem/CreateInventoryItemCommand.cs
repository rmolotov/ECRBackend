using MediatR;

namespace RemoteConfig.Application.InventoryItems.Commands.CreateInventoryItem;

public class CreateInventoryItemCommand : IRequest<string>
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxCount { get; set; }
    public int Price { get; set; }
}