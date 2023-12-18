using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

/// <inheritdoc/>
internal sealed class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _dbContext;

    public PermissionRepository(AppDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc/>
    public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Permissions
                .OrderByDescending(x => x.PermissionName)
                .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        => await _dbContext.Permissions
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);
}