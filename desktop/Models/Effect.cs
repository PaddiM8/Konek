using System;

namespace Konek.Desktop.Models;

public class Effect
{
    public int EffectId { get; init; }

    public TimeOnly StartTime { get; init; }

    public TimeOnly EndTime { get; init; }

    public byte Brightness { get; init; }

    public byte Temperature { get; init; }

    public Effect(TimeOnly startTime, TimeOnly endTime, byte brightness, byte temperature)
    {
        StartTime = startTime;
        EndTime = endTime;
        Brightness = brightness;
        Temperature = temperature;
    }
}