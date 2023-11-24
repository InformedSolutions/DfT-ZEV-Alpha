using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Zev.Core.Domain.Processes.Models;
using Zev.Core.Domain.Vehicles.Models;

namespace Zev.Core.Infrastructure.Persistence;

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