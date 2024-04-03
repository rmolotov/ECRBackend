using AutoMapper;
using RemoteConfig.Application.Enemies.Commands.UpdateEnemy;
using RemoteConfig.Application.Stages.Commands.CreateStage;
using RemoteConfig.Application.Stages.Commands.UpdateStage;
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
        CreateMap<CreateStageCommand, Stage>()
            .ForMember(
                command => command.BoardTiles,
                options => options
                    .MapFrom(tiles => tiles.BoardTiles
                        .Select(tile => new BoardTile
                        {
                            TileType = (BoardTileType)tile[0],
                            TileRotation = (BoardTileRotation)tile[1],
                            Position = new[] { tile[2], tile[3] }
                        })
                        .ToList())
            );
        
        CreateMap<UpdateStageCommand, Stage>()
            .ForMember(
                command => command.BoardTiles,
                options => options
                    .MapFrom(tiles => tiles.BoardTiles
                        .Select(tile => new BoardTile
                        {
                            TileType = (BoardTileType)tile[0],
                            TileRotation = (BoardTileRotation)tile[1],
                            Position = new[] { tile[2], tile[3] }
                        })
                        .ToList())
            );
        
        CreateMap<Stage, GetStageResponse>()
            .ForMember(
                stage => stage.BoardTiles,
                options => options
                    .MapFrom(tiles => tiles.BoardTiles
                        .Select(tile => new[]
                        {
                            (int) tile.TileType,
                            (int) tile.TileRotation,
                            tile.Position[0], tile.Position[1]
                        })
                        .ToList())
            );
    }
}