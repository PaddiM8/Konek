using Konek.Server.Core.Bridges;
using Konek.Server.Core.Exceptions;
using Konek.Server.Core.Repositories;

namespace Konek.Server.Core;

public class DeviceHub
{
    public GroupRepository Groups { get; }

    public LampRepository Lamps { get; }

    public RoutineDefinitionRepository RoutineDefinitions { get; }

    public IEnumerable<RemoteLamp> RemoteLamps
        => _bridge.RemoteLamps ?? throw new BridgeConnectionException();

    private readonly IBridge _bridge;

    public DeviceHub(IBridge bridge)
    {
        _bridge = bridge;
        Lamps = new LampRepository(bridge);
        Groups = new GroupRepository(bridge);
        RoutineDefinitions = new RoutineDefinitionRepository();
    }


    public async Task Load()
    {
        await Lamps.Load();
        await Groups.Load();
    }
}