using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

internal sealed class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _dbContext;

    public PermissionRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Permissions.OrderByDescending(x => x.PermissionName).ToListAsync(cancellationToken);

    public async Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        => await _dbContext.Permissions.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
}