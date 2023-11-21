namespace Zev.Core.Domain.Vehicles;

/// <summary>
/// Represents a repository for managing vehicles.
/// </summary>
public interface IVehicleRepository : IDisposable
{
    /// <summary>
    /// Inserts a list of vehicles into the repository in bulk.
    /// </summary>
    /// <param name="vehicles">The list of vehicles to insert.</param>
    /// <param name="ct">The cancellation token (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task BulkInsertAsync(IList<Vehicle> vehicles, CancellationToken ct = default);


    /// <summary>
    /// Saves changes made to the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Save();
}