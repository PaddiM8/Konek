using Konek.Server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
    {
        if (!Directory.Exists(CommonPaths.DataFolder))
            Directory.CreateDirectory(CommonPaths.DataFolder);

        options.UseSqlite($"Data Source={CommonPaths.Database}");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter>()
            .HaveColumnType("time");
    }
}

public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
        timeOnly => timeOnly.ToTimeSpan(),
        timeSpan => TimeOnly.FromTimeSpan(timeSpan)
    )
    {
    }
}