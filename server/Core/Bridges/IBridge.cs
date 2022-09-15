using System.Collections.Immutable;
using Konek.Server.Core.Models;

namespace Konek.Server.Core.Bridges;

public interface IBridge
{
    public IEnumerable<RemoteLamp> RemoteLamps { get; }

    public void SetBrightness(IEffectBearer effectBearer, byte brightness);

    public void SetTemperature(IEffectBearer effectBearer, byte temperature);
}