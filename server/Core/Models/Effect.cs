using System.ComponentModel.DataAnnotations;

namespace Konek.Server.Core.Models;

public class Effect
{
    [Key]
    public int EffectId { get; init; }

    public TimeOnly StartTime { get; }

    public TimeOnly EndTime { get; }

    public byte Brightness { get; }

    public byte Temperature { get; }

    public Effect(TimeOnly startTime, TimeOnly endTime, byte brightness, byte temperature)
    {
        StartTime = startTime;
        EndTime = endTime;
        Brightness = brightness;
        Temperature = temperature;
    }
}