using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Server.Core.Models;

public class Group : IEffectBearer
{
    [Key]
    public int GroupId { get; init; } = 0;

    public string Name { get; init; }

    public int Priority { get; init; }

    public ICollection<Lamp> Lamps { get; init; } = new List<Lamp>();

    public ICollection<Routine> Routines { get; init; } = new List<Routine>();

    public Group(string name)
    {
        Name = name;
    }
}