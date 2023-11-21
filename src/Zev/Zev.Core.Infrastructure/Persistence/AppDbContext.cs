using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zev.Core.Domain.Processes.Models;
using Zev.Core.Domain.Vehicles;

namespace Zev.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;
    public virtual DbSet<Process> Processes { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public AppDbContext() { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}