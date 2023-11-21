using EFCore.BulkExtensions;
using Serilog;
using Zev.Core.Domain.Vehicles;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

public sealed class VehicleRepository : IVehicleRepository, IDisposable
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    public VehicleRepository(AppDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task BulkInsertAsync(IList<Vehicle> vehicles, CancellationToken ct = default)
    {
        await _context.BulkInsertAsync(vehicles,cancellationToken: ct);
        await _context.BulkInsertAsync(vehicles.Select(x => x.Summary).ToList(), cancellationToken: ct);
        Console.WriteLine("Bulk insert complete.");
        //await Task.WhenAll(primaryInsert, childInsert);
    }

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