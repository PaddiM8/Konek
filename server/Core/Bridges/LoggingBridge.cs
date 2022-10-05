using Konek.Server.Core.Models;
using Microsoft.Extensions.Logging;

namespace Konek.Server.Core.Bridges;

public class LoggingBridge : IBridge
{
    public IEnumerable<RemoteLamp> RemoteLamps { get; }

    private readonly ILogger _logger;

    public LoggingBridge(ILogger logger)
    {
        RemoteLamps = new List<RemoteLamp>()
        {
            new("remoteId1", "Lamp 1"),
            new("remoteId2", "Lamp 2"),
            new("remoteId3", "Lamp 3"),
        };
        _logger = logger;
    }

    public void SetBrightness(IEffectBearer effectBearer, byte brightness)
    {
        _logger.Log(LogLevel.Information, "{}", brightness.ToString());
    }

    public void SetTemperature(IEffectBearer effectBearer, byte temperature)
    {
        _logger.Log(LogLevel.Information, "{}", temperature.ToString());
    }
}