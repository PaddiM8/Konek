using System.ComponentModel.DataAnnotations;

namespace Konek.Server.Core.Models;

public class Lamp : IEffectBearer
{
    [Key]
    public int LampId { get; init; } = 0;

    public string Name { get; init; }

    public string RemoteId { get; init; }

    public ICollection<Group> Groups { get; init; } = new List<Group>();

    public ICollection<Routine> Routines { get; init; } = new List<Routine>();

    public Lamp(string name, string remoteId)
    {
        Name = name;
        RemoteId = remoteId;
    }
}