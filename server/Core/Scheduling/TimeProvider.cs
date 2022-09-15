namespace Konek.Server.Core.Scheduling;

class TimeProvider : ITimeProvider
{
    public DateTime Now
        => DateTime.Now;
}