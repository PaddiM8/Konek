using Konek.Server.Core.Models;
using Microsoft.Extensions.Logging;

namespace Konek.Server.Core.Bridges;

public class LoggingBridge : IBridge
{
    public IEnumerable<RemoteLamp> RemoteLamps { get; } = new List<RemoteLamp>();

    private readonly ILogger _logger;

    public LoggingBridge(ILogger logger)
    {
        _logger = logger;
    }

    public void SetBrightness(IEffectBearer effectBearer, byte brightness)
    {
        _logger.Log(LogLevel.Information, brightness.ToString());
    }

    public void SetTemperature(IEffectBearer effectBearer, byte temperature)
    {
        _logger.Log(LogLevel.Information, temperature.ToString());
    }
}