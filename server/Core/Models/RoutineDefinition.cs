using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Server.Core.Models;

public class RoutineDefinition
{
    [Key]
    public int RoutineDefinitionId { get; } = 0;

    [NotMapped]
    public Effect? CurrentEffect { get; set; }

    public ICollection<Effect> Effects { get; }

    private ICollection<Routine> Routines { get; } = new List<Routine>();

    public RoutineDefinition(ICollection<Effect> effects)
    {
        Effects = effects;
    }
}