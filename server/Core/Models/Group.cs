using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Konek.Server.Core.Models;

public class Group : IEffectBearer
{
    [Key]
    public int GroupId { get; } = 0;

    public string Name { get; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Priority { get; } = 0;

    public ICollection<Lamp> Lamps { get; } = new List<Lamp>();

    public ICollection<Routine> Routines { get; } = new List<Routine>();

    public Group(string name)
    {
        Name = name;
    }
}