
namespace Konek.Server.Core.Scheduling;

record struct TimerPair(Timer Start, Timer End);

class TimerScheduler : IScheduler
{
    private const int MillisecondsPerDay = 1000 * 60 * 60 * 24;
    private readonly Dictionary<int, TimerPair> _timers = new();

    public async ValueTask DisposeAsync()
    {
        foreach (var (_, timerPair) in _timers)
        {
            await timerPair.Start.DisposeAsync();
            await timerPair.End.DisposeAsync();
        }
    }

    public void Schedule<T>(
        int id,
        T item,
        int startInMilliseconds,
        int endInMilliseconds,
        bool repeat,
        Action<T> startCallback,
        Action<T> endCallback)
    {
        var start = new Timer(
            x => startCallback((T)x!),
            item,
            startInMilliseconds,
            repeat ? MillisecondsPerDay : Timeout.Infinite
        );

        async void InitialEndCallback(object? x)
        {
            if (!repeat)
            {
                var timerPair = _timers[id];
                await timerPair.Start.DisposeAsync();
                await timerPair.End.DisposeAsync();
                _timers.Remove(id);
            }

            endCallback(item);
        }

        var end = new Timer(
            InitialEndCallback,
            item,
            endInMilliseconds,
            repeat ? MillisecondsPerDay : Timeout.Infinite
        );

        _timers.Add(id, new TimerPair(start, end));
    }
}