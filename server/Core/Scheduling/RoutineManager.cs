using Konek.Server.Core.Bridges;
using Konek.Server.Core.Models;

namespace Konek.Server.Core.Scheduling;

record RoutineSchedulingArgs(Routine Routine, Effect Effect);

class RoutineManager : IAsyncDisposable
{
    private readonly IEffectBearer _effectBearer;
    private readonly IBridge _bridge;
    private readonly IScheduler _scheduler;
    private readonly ITimeProvider _timeProvider;
    private Routine? _activeRoutine;

    public RoutineManager(
        IEffectBearer effectBearer,
        IBridge bridge,
        IScheduler scheduler,
        ITimeProvider timeProvider)
    {
        _effectBearer = effectBearer;
        _bridge = bridge;
        _scheduler = scheduler;
        _timeProvider = timeProvider;

        foreach (var routine in effectBearer.Routines)
            Schedule(routine);
    }

    public async ValueTask DisposeAsync()
    {
        await _scheduler.DisposeAsync();
    }

    public void Schedule(Routine routine)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        foreach (var effect in routine.Definition.Effects)
        {
            // If the effect would have already been started
            if (now.IsBetween(effect.StartTime, effect.EndTime) &&
                routine.Priority > (_activeRoutine?.Priority ?? -1))
            {
                RunEffect(new(routine, effect));
            }

            int startIn = (int)(effect.StartTime - now).TotalMilliseconds;
            int endIn = (int)(effect.EndTime - now).TotalMilliseconds;
            _scheduler.Schedule(
                effect.EffectId,
                new RoutineSchedulingArgs(routine, effect),
                startIn,
                endIn,
                repeat: routine.Expiry == null,
                RunEffect,
                StopEffect
            );
        }
    }

    private void RunEffect(RoutineSchedulingArgs args)
    {
        var (routine, effect) = args;
        routine.Definition.CurrentEffect = effect;

        if (routine.Priority >= (_activeRoutine?.Priority ?? -1))
        {
            _activeRoutine = routine;
            ApplyEffect(effect);
        }
    }

    private void StopEffect(RoutineSchedulingArgs args)
    {
        var (routine, effect) = args;
        routine.Definition.CurrentEffect = null;

        // If there isn't a new effect lined up in the routine...
        if (routine.Definition.Effects.All(x => x.StartTime != effect.EndTime))
        {
            // ...then if there is a less prioritized routine with an active effect,
            // apply that that effect. There is no need to check for *more*
            // prioritized routines since those would have already been
            // applied, interrupting this one.
            var lowerRoutines = _effectBearer.Routines
                .Where(x => x.Priority < routine.Priority)
                .OrderByDescending(x => x.Priority);
            foreach (var lowerRoutine in lowerRoutines)
            {
                var current = lowerRoutine.Definition.CurrentEffect;
                if (current != null)
                {
                    _activeRoutine = lowerRoutine;
                    ApplyEffect(current);
                    break;
                }
            }

            // If there was no change of routine, turn the lights off
            if (_activeRoutine == args.Routine)
            {
                _bridge.SetBrightness(_effectBearer, 0);
            }
        }

        // If the effect has expired and is the last one in the routine.
        if (routine.Expiry <= _timeProvider.Now &&
            routine.Definition.Effects.All(x => x.EndTime <= effect.EndTime))
        {
            _activeRoutine = null;
            _effectBearer.Routines.Remove(routine);
        }
    }

    private void ApplyEffect(Effect effect)
    {
        // TODO: Fading. It should be able to look at the current
        // time and start at the correct stage.
        _bridge.SetBrightness(_effectBearer, effect.Brightness);
        _bridge.SetTemperature(_effectBearer, effect.Temperature);
    }
}