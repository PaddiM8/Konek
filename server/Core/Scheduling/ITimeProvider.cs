namespace Konek.Server.Core.Scheduling;

interface ITimeProvider
{
    public DateTime Now { get; }
}