using Konek.Server.Core;
using Konek.Server.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Konek.Server.Web.Controllers;

[ApiController]
[Route("routineDefinitions")]
public class RoutineDefinitionController
{
    private readonly DeviceHub _hub;

    public RoutineDefinitionController(DeviceHub hub)
    {
        _hub = hub;
    }

    [HttpPost]
    public async Task Add(ICollection<Effect> effects)
    {
        await _hub.RoutineDefinitions.AddAsync(new RoutineDefinition(effects));
    }
}