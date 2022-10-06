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
    public async Task<IEnumerable<Lamp>> GetAll()
    {
        return await _hub.Lamps.GetAllAsync();
    }

    [HttpGet("/{lampId:int}")]
    public async Task<Lamp?> Get(int lampId)
    {
        return await _hub.Lamps.GetAsync(lampId);
    }

    [HttpPost]
    public async Task<ActionResult<Lamp>> Add(string name, string remoteId)
    {
        var lamp = new Lamp(name, remoteId);
        await _hub.Lamps.AddAsync(lamp);

        return lamp;
    }

    [HttpPost("{lampId:int}/routines")]
    public async Task<ActionResult<Routine>> AddRoutine(int lampId, int routineId)
    {
        var routineDefinition = await _hub.RoutineDefinitions.GetAsync(routineId);
        if (routineDefinition == null)
            return NotFound();

        var routine = new Routine(routineDefinition, null);
        await _hub.Lamps.AddRoutineAsync(lampId, routine);

        return routine;
    }
}