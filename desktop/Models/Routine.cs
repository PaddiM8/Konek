using System;

namespace Konek.Desktop.Models;

public class Routine
{
    public int RoutineId { get; init; } = 0;

    public RoutineDefinition Definition { get; init; }

    public int Priority { get; set; }

    public DateTime? Expiry { get; init; }

    public Routine(RoutineDefinition definition, DateTime? expiry)
    {
        Definition = definition;
        Expiry = expiry;
    }
}