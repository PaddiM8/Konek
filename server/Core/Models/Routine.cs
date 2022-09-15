using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Server.Core.Models;

public class Routine
{
    [Key]
    public int RoutineId { get; } = 0;

    public RoutineDefinition Definition { get; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Priority { get; }

    public DateTime? Expiry { get; }

    public Routine(RoutineDefinition definition, DateTime? expiry, int priority = 0)
    {
        Definition = definition;
        Expiry = expiry;
        Priority = priority;
    }
}