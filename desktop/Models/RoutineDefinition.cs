using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Desktop.Models;

public class RoutineDefinition
{
    public int RoutineDefinitionId { get; init; } = 0;

    public Effect? CurrentEffect { get; set; }

    public ICollection<Effect> Effects { get; init; } = new List<Effect>();

    private ICollection<Routine> Routines { get; init; } = new List<Routine>();

    public RoutineDefinition(ICollection<Effect> effects)
    {
        Effects = effects;
    }
}