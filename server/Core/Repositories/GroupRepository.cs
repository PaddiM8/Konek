using Konek.Server.Core.Bridges;
using Konek.Server.Core.Models;
using Konek.Server.Core.Scheduling;
using Microsoft.EntityFrameworkCore;

namespace Konek.Server.Core.Repositories;

public class GroupRepository : IRepository<Group>
{
    private readonly IBridge _bridge;
    private readonly RoutineManagerBag _routineManagers = new();

    internal GroupRepository(IBridge bridge)
    {
        _bridge = bridge;
    }

    public async Task Load()
    {
        foreach (var group in await GetAllWithRoutinesAsync())
        {
            _routineManagers.CreateManager(group.GroupId, group, _bridge);
        }
    }


    public async Task AddAsync(Group group)
    {
        await using var db = new DeviceContext();
        db.Groups.Add(group);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Group newValue)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(int id)
    {
        await using var db = new DeviceContext();
        var group = await db.Groups.SingleOrDefaultAsync(x => x.GroupId == id);
        if (group != null)
        {
            db.Remove(group);
            await db.SaveChangesAsync();
        }
    }

    public async Task<Group?> GetAsync(int id)
    {
        await using var db = new DeviceContext();

        return await db.Groups
            .Include(x => x.Lamps)
            .Include(x => x.Routines)
            .SingleOrDefaultAsync(x => x.GroupId == id);
    }

    public async Task<List<Group>> GetAllAsync()
    {
        await using var db = new DeviceContext();

        return db.Groups
            .Include(x => x.Lamps)
            .ToList();
    }

    public async Task<List<Group>> GetAllWithRoutinesAsync()
    {
        await using var db = new DeviceContext();

        return db.Groups
            .Include(x => x.Lamps)
            .Include(x => x.Routines)
            .ThenInclude(x => x.Definition)
            .ThenInclude(x => x.Effects)
            .ToList();
    }
}