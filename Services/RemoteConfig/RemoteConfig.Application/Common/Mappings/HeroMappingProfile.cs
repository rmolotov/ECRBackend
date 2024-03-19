using AutoMapper;
using RemoteConfig.Application.Heroes.Commands;
using RemoteConfig.Application.Heroes.Responses;
using RemoteConfig.Core.Entities.Hero;

namespace RemoteConfig.Application.Common.Mappings;

public class HeroMappingProfile : Profile
{
    public HeroMappingProfile()
    {
        ApplyMappings();
    }

    private void ApplyMappings()
    {
        CreateMap<Hero, GetHeroResponse>().ReverseMap();
    }
}