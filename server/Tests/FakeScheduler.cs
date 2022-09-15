using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Konek.Server.Core.Scheduling;

namespace Konek.Server.Tests;

record ScheduleItem<T>(T Value, int Time, Action<T> Callback);

class FakeScheduler : IScheduler
{
    private readonly List<object> _items = new();

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
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
        _items.Add(new ScheduleItem<T>(item, startInMilliseconds, startCallback));
        _items.Add(new ScheduleItem<T>(item, endInMilliseconds, endCallback));
    }

    public void Simulate<T>()
    {
        var sorted = _items
            .Select(x => (ScheduleItem<T>)x)
            .OrderBy(x => x.Time);
        foreach (var item in sorted)
        {
            item.Callback.Invoke(item.Value);
        }
    }
}