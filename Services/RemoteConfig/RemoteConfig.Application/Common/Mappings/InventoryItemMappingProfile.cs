using AutoMapper;
using RemoteConfig.Application.InventoryItems.Responses;
using RemoteConfig.Core.Entities.Inventory;

namespace RemoteConfig.Application.Common.Mappings;

public class InventoryItemMappingProfile : Profile
{
    public InventoryItemMappingProfile()
    {
        ApplyMappings();
    }

    private void ApplyMappings()
    {
        CreateMap<InventoryItem, GetInventoryItemResponse>().ReverseMap();
    }
}