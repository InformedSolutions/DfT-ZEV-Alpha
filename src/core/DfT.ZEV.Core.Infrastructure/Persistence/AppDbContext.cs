using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Vehicles.Models;

namespace DfT.ZEV.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;
    public virtual DbSet<Process> Processes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.IsInMemory())
        {
            modelBuilder.Ignore<JsonDocument>();
        }
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}