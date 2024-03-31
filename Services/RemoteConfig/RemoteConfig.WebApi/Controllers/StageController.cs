using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteConfig.Application.Stages.Commands.CreateStage;
using RemoteConfig.Application.Stages.Commands.DeleteStage;
using RemoteConfig.Application.Stages.Commands.UpdateStage;
using RemoteConfig.Application.Stages.Queries;
using RemoteConfig.Application.Stages.Responses;

namespace RemoteConfig.WebApi.Controllers;

[ApiVersionNeutral]
[AllowAnonymous]
public class StageController(IMapper mapper) : BaseController
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet]
    public async Task<ActionResult<IList<GetStageResponse>>> GetAll()
    {
        var query = new GetStagesListQuery();
        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetStageResponse>> GetStage(string id)
    {
        var query = new GetStageQuery(id);
        var response = await Mediator.Send(query);

        return Ok(response);
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateStage([FromBody] CreateStageCommand command)
    {
        var id = await Mediator.Send(command);

        return Created($"[/api/stages/{id}]", id);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    public async Task<ActionResult> UpdateStage([FromBody] UpdateStageCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    public async Task<ActionResult> DeleteStage([FromBody] string id)
    {
        var command = new DeleteStageCommand { Id = id };
        await Mediator.Send(command);

        return NoContent();
    }
}