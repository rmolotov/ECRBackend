using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteConfig.Application.Enemies.Commands.CreateEnemy;
using RemoteConfig.Application.Enemies.Commands.DeleteEnemy;
using RemoteConfig.Application.Enemies.Commands.UpdateEnemy;
using RemoteConfig.Application.Enemies.Queries;
using RemoteConfig.Application.Enemies.Responses;

namespace RemoteConfig.WebApi.Controllers;

[ApiVersionNeutral]
public class EnemyController(IMapper mapper) : BaseController
{
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet]
    public async Task<ActionResult<IList<GetEnemyResponse>>> GetAll()
    {
        var query = new GetEnemiesListQuery();
        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetEnemyResponse>> GetEnemy(string id)
    {
        var query = new GetEnemyQuery(id);
        var response = await Mediator.Send(query);

        return Ok(response);
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateEnemy([FromBody] CreateEnemyCommand command)
    {
        var id = await Mediator.Send(command);

        return Created($"[/api/enemy/{id}]", id);
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    public async Task<ActionResult> UpdateEnemy([FromBody] UpdateEnemyCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    public async Task<ActionResult> DeleteEnemy([FromBody] string id)
    {
        var command = new DeleteEnemyCommand() { Id = id };
        await Mediator.Send(command);

        return NoContent();
    }
}