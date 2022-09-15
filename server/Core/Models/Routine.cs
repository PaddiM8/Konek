using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Server.Core.Models;

public class Routine
{
    [Key]
    public int RoutineId { get; init; } = 0;

    public RoutineDefinition Definition { get; init; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Priority { get; init; }

    public DateTime? Expiry { get; init; }

    public Routine(RoutineDefinition definition, DateTime? expiry, int priority = 0)
    {
        Definition = definition;
        Expiry = expiry;
        Priority = priority;
    }

    public Routine(DateTime? expiry, int priority = 0)
    {
        Expiry = expiry;
        Priority = priority;
    }
}