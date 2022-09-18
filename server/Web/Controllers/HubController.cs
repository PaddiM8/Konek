using Konek.Server.Core;
using Konek.Server.Core.Bridges;
using Microsoft.AspNetCore.Mvc;

namespace Konek.Server.Web.Controllers;

[ApiController]
[Route("hub")]
public class HubController : ControllerBase
{
    private readonly DeviceHub _hub;

    public HubController(DeviceHub hub)
    {
        _hub = hub;
    }

    [HttpGet]
    public IEnumerable<RemoteLamp> Get()
    {
        return _hub.RemoteLamps;
    }
}