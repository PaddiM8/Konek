using Konek.Server.Core.Bridges;
using Konek.Server.Core.Models;
using Konek.Server.Core.Scheduling;
using Microsoft.EntityFrameworkCore;

namespace Konek.Server.Core.Repositories;

public class GroupRepository : IRepository<Group>
{
    private readonly IBridge _bridge;
    private readonly RoutineManagerBag _routineManagers;

    private GroupRepository(IBridge bridge, RoutineManagerBag routineManagers)
    {
        _bridge = bridge;
        _routineManagers = routineManagers;
    }

    internal static async Task<GroupRepository> CreateAsync(IBridge bridge)
    {
        var routineManagers = new RoutineManagerBag();
        var repository = new GroupRepository(bridge, routineManagers);
        foreach (var group in await repository.GetAllWithRoutinesAsync())
        {
            routineManagers.CreateManager(group.GroupId, group, bridge);
        }

        return repository;
    }

    public async Task AddAsync(Group group)
    {
        await using var db = new DeviceContext();
        db.Groups.Add(group);
        await db.SaveChangesAsync();
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