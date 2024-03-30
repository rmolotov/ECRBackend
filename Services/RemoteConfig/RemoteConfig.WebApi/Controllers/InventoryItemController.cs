using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteConfig.Application.InventoryItems.Commands.CreateInventoryItem;
using RemoteConfig.Application.InventoryItems.Commands.DeleteInventoryItem;
using RemoteConfig.Application.InventoryItems.Commands.UpdateInventoryItem;
using RemoteConfig.Application.InventoryItems.Queries;
using RemoteConfig.Application.InventoryItems.Responses;

namespace RemoteConfig.WebApi.Controllers;

[ApiVersionNeutral]
[AllowAnonymous]
public class InventoryItemController(IMapper mapper) : BaseController
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet]
    public async Task<ActionResult<IList<GetInventoryItemResponse>>> GetAll()
    {
        var query = new GetInventoryItemsListQuery();
        var response = await Mediator.Send(query);

        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetInventoryItemResponse>> GetInventoryItem(string id)
    {
        var query = new GetInventoryItemQuery(id);
        var response = await Mediator.Send(query);

        return Ok(response);
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateInventoryItem([FromBody] CreateInventoryItemCommand command)
    {
        var id = await Mediator.Send(command);

        return Created($"[/api/inventoryItems/{id}]", id);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPut]
    public async Task<ActionResult> UpdateInventoryItem([FromBody] UpdateInventoryItemCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    public async Task<ActionResult> DeleteInventoryItem([FromBody] string id)
    {
        var command = new DeleteInventoryItemCommand { Id = id };
        await Mediator.Send(command);

        return NoContent();
    }
}