using AutoMapper;
using RemoteConfig.Application.Enemies.Responses;
using RemoteConfig.Core.Entities.Enemy;

namespace RemoteConfig.Application.Common.Mappings;

public class EnemyMappingProfile : Profile
{
    public EnemyMappingProfile()
    {
        ApplyMappings();
    }

    private void ApplyMappings()
    {
        CreateMap<Enemy, GetEnemyResponse>().ReverseMap();
    }
}