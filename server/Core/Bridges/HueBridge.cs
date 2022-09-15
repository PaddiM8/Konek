using System.Collections.Immutable;
using System.Net;
using Konek.Server.Core.Models;
using Q42.HueApi;

namespace Konek.Server.Core.Bridges;

class HueBridge : IBridge
{
    public IEnumerable<RemoteLamp> RemoteLamps { get; }

    private readonly LocalHueClient _client;
    private readonly Dictionary<string, Light> _hueLamps = new();

    private HueBridge(LocalHueClient client, IEnumerable<Light> hueLamps)
    {
        _client = client;

        var remoteLamps = new List<RemoteLamp>();
        foreach (var hueLamp in hueLamps)
        {
            remoteLamps.Add(new RemoteLamp(hueLamp.Id, hueLamp.Name));
            _hueLamps[hueLamp.Id] = hueLamp;
        }

        RemoteLamps = remoteLamps;
    }

    public static async Task<HueBridge?> InitializeAsync(string key)
    {
        var locator = new HttpBridgeLocator();
        var bridge  = (await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5))).First();
        var client = new LocalHueClient(bridge.IpAddress);

        client.Initialize(key);

        return client.IsInitialized
            ? new HueBridge(client, await client.GetLightsAsync())
            : null;
    }

    public static async Task<HueBridge?> RegisterAsync()
    {
        var locator = new HttpBridgeLocator();
        var bridge  = (await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5))).First();
        var client = new LocalHueClient(bridge.IpAddress);
        string? appKey = await client.RegisterAsync("konek", Dns.GetHostName());

        if (appKey == null)
            return null;

        await File.WriteAllTextAsync(CommonPaths.AppKey, appKey);

        return new HueBridge(client, await client.GetLightsAsync());
    }

    public void SetBrightness(IEffectBearer effectBearer, byte brightness)
    {
        string hueId = (effectBearer as Lamp)?.RemoteId ?? throw new NotImplementedException();
        _hueLamps[hueId].State.Brightness = brightness;
    }

    public void SetTemperature(IEffectBearer effectBearer, byte temperature)
    {
        string hueId = (effectBearer as Lamp)?.RemoteId ?? throw new NotImplementedException();
        _hueLamps[hueId].State.ColorTemperature = temperature;
    }
}