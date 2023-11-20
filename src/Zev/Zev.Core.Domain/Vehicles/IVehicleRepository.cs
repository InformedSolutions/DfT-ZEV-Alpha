namespace Zev.Core.Domain.Vehicles;

public interface IVehicleRepository : IDisposable    
{
    public Task BulkInsertAsync(IEnumerable<Vehicle> vehicles, CancellationToken ct = default);
    public Task Save();
}