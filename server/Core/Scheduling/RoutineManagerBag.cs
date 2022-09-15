using System.Collections;
using Konek.Server.Core.Bridges;
using Konek.Server.Core.Models;

namespace Konek.Server.Core.Scheduling;

class RoutineManagerBag : IEnumerable<RoutineManager>
{
    private readonly Dictionary<int, RoutineManager> _routineManagers = new();

    public RoutineManager? this[int id]
    {
        get
        {
            _routineManagers.TryGetValue(id, out var result);

            return result;
        }
    }

    public void CreateManager(int id, IEffectBearer effectBearer, IBridge bridge)
    {
        _routineManagers.Add(id, new RoutineManager(
            effectBearer,
            bridge,
            new TimerScheduler(),
            new TimeProvider()
        ));
    }

    public async Task ClearAsync()
    {
        foreach (var (_, scheduler) in _routineManagers)
            await scheduler.DisposeAsync();

        _routineManagers.Clear();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var scheduler = this[id];
        if (scheduler == null)
            return false;

        await scheduler.DisposeAsync();
        _routineManagers.Remove(id);

        return true;
    }

    public IEnumerator<RoutineManager> GetEnumerator()
        => _routineManagers.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}