using EFCore.BulkExtensions;
using Zev.Core.Domain.Vehicles.Models;
using Zev.Core.Domain.Vehicles.Services;
using Zev.Core.Infrastructure.Persistence;

namespace Zev.Core.Infrastructure.Repositories;

/// <inheritdoc cref="IVehicleRepository" />
public sealed class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;

    private bool _disposed;

    /// <summary>
    ///     This method initializes a new instance of the <see cref="VehicleRepository" /> class.
    /// </summary>
    /// <param name="context">AppDbContext</param>
    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task BulkInsertAsync(IList<Vehicle> vehicles, CancellationToken ct = default)
    {
        await _context.BulkInsertAsync(vehicles, cancellationToken: ct);
        await _context.BulkInsertAsync(vehicles.Select(x => x.Summary).ToList(), cancellationToken: ct);
    }

    /// <inheritdoc />
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }
}