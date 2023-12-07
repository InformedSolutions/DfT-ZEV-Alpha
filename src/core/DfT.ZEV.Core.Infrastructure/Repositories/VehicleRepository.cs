using EFCore.BulkExtensions;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using DfT.ZEV.Core.Domain.Vehicles.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

/// <inheritdoc cref="IVehicleRepository" />
internal sealed class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;


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

    public async Task<IEnumerable<Vehicle>> GetVehiclesByManufacturerNameAsync(string manufacturerName, int pageNumber, int pageSize, CancellationToken ct = default)
        => await _context.Vehicles
            .Where(x => x.Mh == manufacturerName)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
}