using Konek.Server.Core.Bridges;

namespace Konek.Server.Web;

class BridgeSetup
{
    public static async Task<HueBridge> FindHueBridge()
    {
        var bridge = await HueBridge.InitializeAsync();
        if (bridge != null)
            return bridge;

        Console.WriteLine("Hue Bridge not registered. Press the link button and then press any key to continue pairing.");
        Console.ReadKey();
        Console.WriteLine("Registering bridge...");

        bridge = await HueBridge.RegisterAsync();
        if (bridge != null)
        {
            Console.WriteLine("Success!");

            return bridge;
        }

        throw new Exception("Could not connect to bridge");
    }
}