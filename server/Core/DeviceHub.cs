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

    private DeviceHub(IBridge bridge,
        LampRepository lampRepository,
        GroupRepository groupRepository,
        RoutineDefinitionRepository routineDefinitionRepository)
    {
        _bridge = bridge;
        Lamps = lampRepository;
        Groups = groupRepository;
        RoutineDefinitions = routineDefinitionRepository;
    }

    private static async Task<DeviceHub> CreateAsync(IBridge bridge)
    {
        var lampRepository = await LampRepository.CreateAsync(bridge);
        var groupRepository = await GroupRepository.CreateAsync(bridge);

        return new DeviceHub(
            bridge,
            lampRepository,
            groupRepository,
            new RoutineDefinitionRepository()
        );
    }

    public static async Task<DeviceHub?> Initialize()
    {
        if (!Directory.Exists(CommonPaths.DataFolder))
            Directory.CreateDirectory(CommonPaths.DataFolder);

        HueBridge? bridge = null;
        if (File.Exists(CommonPaths.AppKey))
            bridge = await HueBridge.InitializeAsync(await File.ReadAllTextAsync(CommonPaths.AppKey));

        return bridge == null
            ? null
            : await CreateAsync(bridge);
    }

    public static async Task<DeviceHub?> RegisterHueAsync()
    {
        var bridge = await HueBridge.RegisterAsync();

        return bridge == null
            ? null
            : await CreateAsync(bridge);
    }
}