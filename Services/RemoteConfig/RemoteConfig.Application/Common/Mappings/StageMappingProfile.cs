using AutoMapper;
using RemoteConfig.Application.Stages.Responses;
using RemoteConfig.Core.Entities.Stage;

namespace RemoteConfig.Application.Common.Mappings;

public class StageMappingProfile : Profile
{
    public StageMappingProfile()
    {
        ApplyMappings();
    }
    
    private void ApplyMappings()
    {
        CreateMap<Stage, GetStageResponse>().ReverseMap();
    }
}