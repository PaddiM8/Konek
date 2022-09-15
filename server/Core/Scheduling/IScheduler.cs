namespace Konek.Server.Core.Scheduling;

interface IScheduler : IAsyncDisposable
{
    public void Schedule<T>(
        int id,
        T item,
        int startInMilliseconds,
        int endInMilliseconds,
        bool repeat,
        Action<T> startCallback,
        Action<T> endCallback);
}