using Konek.Server.Core.Bridges;
using Konek.Server.Core.Models;
using Konek.Server.Core.Scheduling;
using Microsoft.EntityFrameworkCore;

namespace Konek.Server.Core.Repositories;

public class LampRepository : IRepository<Lamp>
{
    private readonly IBridge _bridge;
    private readonly RoutineManagerBag _routineManagers;

    private LampRepository(IBridge bridge, RoutineManagerBag routineManagers)
    {
        _bridge = bridge;
        _routineManagers = routineManagers;
    }

    internal static async Task<LampRepository> CreateAsync(IBridge bridge)
    {
        var routineManagers = new RoutineManagerBag();
        var repository = new LampRepository(bridge, routineManagers);
        foreach (var lamp in await repository.GetAllWithRoutinesAsync())
        {
            routineManagers.CreateManager(lamp.LampId, lamp, bridge);
        }

        return repository;
    }

    public async Task AddAsync(Lamp lamp)
    {
        await using var db = new DeviceContext();
        db.Lamps.Add(lamp);
        await db.SaveChangesAsync();
        _routineManagers.CreateManager(lamp.LampId, lamp, _bridge);
    }

    public async Task AddRoutine(int lampId, Routine routine)
    {
        await using var db = new DeviceContext();
        var lamp = await db.Lamps.FindAsync(lampId);
        if (lamp != null)
        {
            lamp.Routines.Add(routine);
            await db.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(int id)
    {
        await using var db = new DeviceContext();
        var lamp = await db.Lamps.SingleOrDefaultAsync(x => x.LampId == id);
        if (lamp != null)
        {
            db.Remove(lamp);
            await db.SaveChangesAsync();
        }

        // TODO: Remove scheduler and make sure all the routine instances are removed
    }

    public async Task<Lamp?> GetAsync(int id)
    {
        await using var db = new DeviceContext();

        return await db.Lamps
            .Include(x => x.Routines)
            .ThenInclude(x => x.Definition)
            .ThenInclude(x => x.Effects)
            .SingleOrDefaultAsync(x => x.LampId == id);
    }

    public async Task<List<Lamp>> GetAllAsync()
    {
        await using var db = new DeviceContext();

        return db.Lamps.ToList();
    }

    public async Task<List<Lamp>> GetAllWithRoutinesAsync()
    {
        await using var db = new DeviceContext();

        return db.Lamps
            .Include(x => x.Routines)
            .ThenInclude(x => x.Definition)
            .ThenInclude(x => x.Effects)
            .ToList();
    }
}