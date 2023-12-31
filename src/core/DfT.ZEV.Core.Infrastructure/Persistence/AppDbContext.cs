using System.Reflection;
using System.Text.Json;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Manufacturers.Models;
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
    
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
    public virtual DbSet<ManufacturerPool> ManufacturerPools { get; set; } = null!;
    public virtual DbSet<InternalManufacturerActivity> InternalManufacturerActivities { get; set; } = null!;
    public virtual DbSet<ManufacturerTradingActivity> ManufacturerTradingActivities { get; set; } = null!;
   
    public virtual DbSet<Permission> Permissions { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.IsInMemory())
        {
            modelBuilder.Ignore<JsonDocument>();
        }
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

       modelBuilder.Entity<Permission>()
           .HasData(
               new Permission("Permission1"), 
               new Permission("Permission2"), 
               new Permission("Permission3"),
               new Permission("Permission4"),
               new Permission("Permission5"),
               new Permission("Permission6")
               );
    }
}