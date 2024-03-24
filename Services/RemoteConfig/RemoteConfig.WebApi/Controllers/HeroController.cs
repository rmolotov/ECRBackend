using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteConfig.Application.Heroes.Commands.CreateHero;
using RemoteConfig.Application.Heroes.Commands.DeleteHero;
using RemoteConfig.Application.Heroes.Commands.UpdateHero;
using RemoteConfig.Application.Heroes.Queries;
using RemoteConfig.Application.Heroes.Responses;

namespace RemoteConfig.WebApi.Controllers;

[ApiVersionNeutral]
[AllowAnonymous]
public class HeroController(IMapper mapper) : BaseController
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet]
    public async Task<ActionResult<IList<GetHeroResponse>>> GetAll()
    {
        var query = new GetHeroesListQuery();
        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetHeroResponse>> GetHero(string id)
    {
        var query = new GetHeroQuery(id);
        var response = await Mediator.Send(query);

        return Ok(response);
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateHero([FromBody] CreateHeroCommand command)
    {
        var id = await Mediator.Send(command);

        return Created($"[/api/heroes/{id}]", id);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    public async Task<ActionResult> UpdateHero([FromBody] UpdateHeroCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    public async Task<ActionResult> DeleteHero([FromBody] string id)
    {
        var command = new DeleteHeroCommand { Id = id };
        await Mediator.Send(command);

        return NoContent();
    }
}