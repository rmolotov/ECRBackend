using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteConfig.Application.Enemies.Queries;
using RemoteConfig.Application.Enemies.Responses;

namespace RemoteConfig.WebApi.Controllers;

[ApiVersionNeutral]
[AllowAnonymous]
public class EnemyController(IMapper mapper) : BaseController
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet]
    public async Task<ActionResult<IList<GetEnemyResponse>>> GetAll()
    {
        var query = new GetEnemiesListQuery();
        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetEnemyResponse>> GetEnemy(string id)
    {
        var query = new GetEnemyQuery(id);
        var response = await Mediator.Send(query);

        return Ok(response);
    }
}