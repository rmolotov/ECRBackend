namespace RemoteConfig.Application.InventoryItems.Responses;

public class GetInventoryItemResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxCount { get; set; }
    public int Price { get; set; }
}