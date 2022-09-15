using Konek.Server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Konek.Server.Core;

class DeviceContext : DbContext
{
    public DbSet<Group> Groups
        => Set<Group>();

    public DbSet<Lamp> Lamps
        => Set<Lamp>();

    public DbSet<RoutineDefinition> RoutineDefinitions
        => Set<RoutineDefinition>();

    public DbSet<Routine> Routines
        => Set<Routine>();

    public DbSet<Effect> Effects
        => Set<Effect>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={CommonPaths.Database}");
}