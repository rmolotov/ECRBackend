using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}