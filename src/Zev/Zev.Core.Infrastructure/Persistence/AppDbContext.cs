using Microsoft.EntityFrameworkCore;

namespace Zev.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    
    
    public AppDbContext() { }
    public AppDbContext(string connectionString) : base(GetOptions(connectionString)) { }

    private static DbContextOptions GetOptions(string connectionString)
    {
        return new DbContextOptionsBuilder().UseNpgsql(connectionString).Options;
    }
}