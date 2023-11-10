using Microsoft.EntityFrameworkCore;
using Zev.Core.Domain.Examples;

namespace Zev.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<ExampleModel> ExampleModels { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
    }
}