using System.Text.Json.Nodes;
using Konek.Server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Konek.Server.Core.Repositories;

public class RoutineDefinitionRepository : IRepository<RoutineDefinition>
{
    public async Task AddAsync(RoutineDefinition routineDefinition)
    {
        await using var db = new DeviceContext();
        db.RoutineDefinitions.Add(routineDefinition);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(RoutineDefinition newValue)
    {
        await using var db = new DeviceContext();
        db.RoutineDefinitions.Update(newValue);
        await db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        await using var db = new DeviceContext();
        var routineDefinition = await db.RoutineDefinitions.SingleOrDefaultAsync(x => x.RoutineDefinitionId == id);
        if (routineDefinition != null)
        {
            db.Remove(routineDefinition);
            await db.SaveChangesAsync();
        }
    }

    public async Task<RoutineDefinition?> GetAsync(int id)
    {
        await using var db = new DeviceContext();

        return await db.RoutineDefinitions
            .Include(x => x.Effects)
            .SingleOrDefaultAsync(x => x.RoutineDefinitionId == id);
    }

    public async Task<List<RoutineDefinition>> GetAllAsync()
    {
        await using var db = new DeviceContext();

        return db.RoutineDefinitions
            .Include(x => x.Effects)
            .ToList();
    }
}