using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Server.Core.Models;

public class RoutineDefinition
{
    [Key]
    public int RoutineDefinitionId { get; init; } = 0;

    [NotMapped]
    public Effect? CurrentEffect { get; set; }

    public ICollection<Effect> Effects { get; init; } = new List<Effect>();

    private ICollection<Routine> Routines { get; init; } = new List<Routine>();

    public RoutineDefinition()
    {
    }

    public RoutineDefinition(ICollection<Effect> effects)
    {
        Effects = effects;
    }
}