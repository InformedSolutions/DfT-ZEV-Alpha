using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DfT.ZEV.Core.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    ///     This method is called by the EF Core CLI tools when running migrations.
    /// </summary>
    /// <param name="args">CLI arguments</param>
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString(args);

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    private static string GetConnectionString(IReadOnlyList<string> args)
    {
        var connectionString = args.Count > 0 ? args[0] : null;
        connectionString ??= Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

        if (connectionString is null or "") throw new ArgumentNullException(nameof(connectionString));

        return connectionString;
    }
}