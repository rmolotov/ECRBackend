using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}