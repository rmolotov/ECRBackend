using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RemoteConfig.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/{version:apiVersion}/[controller]/")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}