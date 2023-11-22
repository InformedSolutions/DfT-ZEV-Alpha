using EFCore.BulkExtensions;
using Serilog;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Domain.Vehicles.Models;
using Zev.Core.Domain.Vehicles.Services;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

/// <inheritdoc cref="IVehicleRepository"/>
public sealed class VehicleRepository : IVehicleRepository, IDisposable
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    
    /// <summary>
    /// This method initializes a new instance of the <see cref="VehicleRepository"/> class.
    /// </summary>
    /// <param name="context">AppDbContext</param>
    /// <param name="logger">ILogger</param>
    public VehicleRepository(AppDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task BulkInsertAsync(IList<Vehicle> vehicles, CancellationToken ct = default)
    {
        await _context.BulkInsertAsync(vehicles,cancellationToken: ct);
        await _context.BulkInsertAsync(vehicles.Select(x => x.Summary).ToList(), cancellationToken: ct);
    }

    /// <inheritdoc/>
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
    
    private bool _disposed = false;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}