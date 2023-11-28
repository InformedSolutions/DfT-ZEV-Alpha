using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DfT.ZEV.Core.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async ValueTask<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Users.Include(x => x.RolesBridges)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async ValueTask<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Users.ToListAsync(cancellationToken);

    public async ValueTask<IEnumerable<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _dbContext.Users.OrderByDescending(x => x.CreatedAt).Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);

    public async Task InsertAsync(User user, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AddAsync(user, cancellationToken);

    public void Update(User user)
        => _dbContext.Users.Update(user);

    public void Delete(User user)
        =>  _dbContext.Remove(user);
}