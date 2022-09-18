using Konek.Server.Core;
using Konek.Server.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Konek.Server.Web.Controllers;

[ApiController]
[Route("lamps")]
public class LampController : ControllerBase
{
    private readonly DeviceHub _hub;

    public LampController(DeviceHub hub)
    {
        _hub = hub;
    }

    [HttpGet]
    public async Task<IEnumerable<Lamp>> Get()
    {
        return await _hub.Lamps.GetAllWithRoutinesAsync();
    }

    [HttpPost]
    public async Task Add(string name, string remoteId)
    {
        await _hub.Lamps.AddAsync(new Lamp(name, remoteId));
    }

    [HttpPost("{lampId}/routines")]
    public async Task<IActionResult> AddRoutine(int lampId, int routineId)
    {
        var routineDefinition = await _hub.RoutineDefinitions.GetAsync(routineId);
        if (routineDefinition == null)
            return NotFound();

        await _hub.Lamps.AddRoutineAsync(lampId, new Routine(routineDefinition, null));

        return Ok();
    }
}