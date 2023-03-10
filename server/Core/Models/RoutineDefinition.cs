using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Konek.Server.Core.Models;

public class RoutineDefinition
{
    [Key]
    public int RoutineDefinitionId { get; init; } = 0;

    public string Name { get; init; }

    [NotMapped]
    public Effect? CurrentEffect { get; set; }

    public ICollection<Effect> Effects { get; init; }

    private ICollection<Routine> Routines { get; init; } = new List<Routine>();

    public RoutineDefinition(string name)
    {
        Name = name;
        Effects = new List<Effect>();
    }

    [Newtonsoft.Json.JsonConstructor]
    [JsonConstructor]
    public RoutineDefinition(string name, ICollection<Effect> effects)
    {
        Name = name;
        Effects = effects;
    }
}