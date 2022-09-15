using Konek.Server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Konek.Server.Core.Repositories;

public class EffectRepository : IRepository<Effect>
{
    public async Task AddAsync(Effect effect)
    {
        await using var db = new DeviceContext();
        db.Effects.Add(effect);
        await db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        await using var db = new DeviceContext();
        var effect = await db.Effects.SingleOrDefaultAsync(x => x.EffectId == id);
        if (effect != null)
        {
            db.Remove(effect);
            await db.SaveChangesAsync();
        }
    }

    public async Task<Effect?> GetAsync(int id)
    {
        await using var db = new DeviceContext();

        return await db.Effects
            .SingleOrDefaultAsync(x => x.EffectId == id);
    }

    public async Task<List<Effect>> GetAllAsync()
    {
        await using var db = new DeviceContext();

        return db.Effects
            .ToList();
    }
}