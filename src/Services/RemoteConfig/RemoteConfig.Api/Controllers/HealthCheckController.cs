using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RemoteConfig.WebApi.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi=true)]
[Route("[controller]")]
[AllowAnonymous]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public string Check() =>
        "Service is online";
}