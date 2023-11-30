using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Services;

public interface IPermissionRepository
{
    public Task<IEnumerable<Permission> > GetAllAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}