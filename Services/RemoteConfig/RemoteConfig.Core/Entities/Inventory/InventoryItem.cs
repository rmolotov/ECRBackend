namespace RemoteConfig.Core.Entities.Inventory;

public class InventoryItem : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxCount { get; set; }
    public int Price { get; set; }
}