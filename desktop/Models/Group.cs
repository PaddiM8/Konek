using System.Collections.Generic;

namespace Konek.Desktop.Models;

public class Group
{
    public int GroupId { get; init; } = 0;

    public string Name { get; init; }

    public int Priority { get; init; } = 0;

    public ICollection<Lamp> Lamps { get; init; } = new List<Lamp>();

    public ICollection<Routine> Routines { get; init; } = new List<Routine>();

    public Group(string name)
    {
        Name = name;
    }
}