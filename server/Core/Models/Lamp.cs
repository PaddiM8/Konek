using System.ComponentModel.DataAnnotations;

namespace Konek.Server.Core.Models;

public class Lamp : IEffectBearer
{
    [Key]
    public int LampId { get; } = 0;

    public string Name { get; }

    public string RemoteId { get; }

    public ICollection<Group> Groups { get; } = new List<Group>();

    public ICollection<Routine> Routines { get; } = new List<Routine>();

    public Lamp(string name, string remoteId)
    {
        Name = name;
        RemoteId = remoteId;
    }
}