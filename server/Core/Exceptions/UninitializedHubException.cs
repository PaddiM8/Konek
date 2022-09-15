namespace Konek.Server.Core.Exceptions;

public class UninitializedHubException : Exception
{
    public UninitializedHubException()
    {
    }

    public UninitializedHubException(string message)
        : base(message)
    {
    }
}