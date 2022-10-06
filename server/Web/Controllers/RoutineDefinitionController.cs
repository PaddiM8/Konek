using Konek.Server.Core;
using Konek.Server.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Konek.Server.Web.Controllers;

[ApiController]
[Route("routineDefinitions")]
public class RoutineDefinitionController : ControllerBase
{
    private readonly DeviceHub _hub;

    public RoutineDefinitionController(DeviceHub hub)
    {
        _hub = hub;
    }

    [HttpGet]
    public async Task<IEnumerable<RoutineDefinition>> GetAll()
    {
        return await _hub.RoutineDefinitions.GetAllAsync();
    }

    [HttpPost]
    public async Task<ActionResult<RoutineDefinition>> Add(string name, [FromBody] ICollection<Effect> effects)
    {
        var routineDefinition = new RoutineDefinition(name, effects);
        await _hub.RoutineDefinitions.AddAsync(routineDefinition);

        return routineDefinition;
    }
}