using Microsoft.AspNetCore.Mvc;

namespace SuperStore.Api.Ping;

[ApiController]
[Route("ping")]
public class PingController : ControllerBase
{
    [HttpGet(Name = "ping")]
    public string Get() => "pong";
}