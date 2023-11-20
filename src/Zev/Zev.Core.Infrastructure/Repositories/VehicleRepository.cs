using EFCore.BulkExtensions;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository, IDisposable
{
    private readonly AppDbContext _context;

    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task BulkInsertAsync(IEnumerable<Vehicle> vehicles, CancellationToken ct = default)
    {
        await _context.BulkInsertAsync(vehicles.ToList(), cancellationToken: ct);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
    
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
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