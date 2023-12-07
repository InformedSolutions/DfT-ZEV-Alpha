using DfT.ZEV.Core.Domain.Vehicles.Models;

namespace DfT.ZEV.Core.Domain.Vehicles.Services;

/// <summary>
///     Represents a repository for managing vehicles.
/// </summary>
public interface IVehicleRepository
{
    /// <summary>
    ///     Inserts a list of vehicles into the repository in bulk.
    /// </summary>
    /// <param name="vehicles">The list of vehicles to insert.</param>
    /// <param name="ct">The cancellation token (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task BulkInsertAsync(IList<Vehicle> vehicles, CancellationToken ct = default);

    public Task<IEnumerable<Vehicle>> GetVehiclesByManufacturerNameAsync(string manufacturerName, int pageNumber, int pageSize, CancellationToken ct = default);
}