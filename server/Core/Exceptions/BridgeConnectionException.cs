namespace Konek.Server.Core.Exceptions;

public class BridgeConnectionException : Exception
{
    public BridgeConnectionException()
    {
    }

    public BridgeConnectionException(string message)
        : base(message)
    {
    }
}