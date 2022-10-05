using Konek.Server.Core;
using Konek.Server.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Konek.Server.Web.Controllers;

[ApiController]
[Route("groups")]
public class GroupController : ControllerBase
{
    private readonly DeviceHub _hub;

    public GroupController(DeviceHub hub)
    {
        _hub = hub;
    }

    [HttpGet]
    public async Task<IEnumerable<Group>> Get()
    {
        return await _hub.Groups.GetAllWithRoutinesAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Group>> Add(string name)
    {
        var group = new Group(name);
        await _hub.Groups.AddAsync(group);

        return group;
    }
}