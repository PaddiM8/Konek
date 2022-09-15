namespace Konek.Server.Core.Models;

public interface IEffectBearer
{
    public ICollection<Routine> Routines { get; }
}