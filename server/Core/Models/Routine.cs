using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Server.Core.Models;

public class Routine
{
    [Key]
    public int RoutineId { get; init; } = 0;

    public RoutineDefinition Definition { get; init; } = null!;

    public int Priority { get; set; }

    public DateTime? Expiry { get; init; }

    public Routine(RoutineDefinition definition, DateTime? expiry)
    {
        Definition = definition;
        Expiry = expiry;
    }

    public Routine(DateTime? expiry)
    {
        Expiry = expiry;
    }
}