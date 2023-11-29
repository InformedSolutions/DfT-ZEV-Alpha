using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Services;

public interface IManufacturerRepository
{
    public ValueTask<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public ValueTask<Manufacturer?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public ValueTask<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default);
    public ValueTask<IEnumerable<Manufacturer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    public Task InsertAsync (Manufacturer manufacturer, CancellationToken cancellationToken = default);
    public void Update (Manufacturer manufacturer);
    public void Delete (Manufacturer manufacturer);
}