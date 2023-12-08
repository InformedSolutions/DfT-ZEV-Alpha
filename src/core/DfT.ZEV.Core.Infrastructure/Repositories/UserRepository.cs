using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

/// <inheritdoc/>
internal sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc/>
    public async ValueTask<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.ManufacturerBridges)
                    .ThenInclude(x => x.Manufacturer)
                .Include(x => x.ManufacturerBridges)
                    .ThenInclude(x => x.Permissions)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async ValueTask<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.ManufacturerBridges)
                    .ThenInclude(x => x.Manufacturer)
                .Include(x => x.ManufacturerBridges)
                    .ThenInclude(x => x.Permissions)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async ValueTask<IEnumerable<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _dbContext.Users
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task InsertAsync(User user, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AddAsync(user, cancellationToken);

    /// <inheritdoc/>
    public void Update(User user)
        => _dbContext.Users.Update(user);

    /// <inheritdoc/>
    public void Delete(User user)
        => _dbContext.Remove(user);
}