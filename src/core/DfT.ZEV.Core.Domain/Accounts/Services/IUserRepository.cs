using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Services;

public interface IUserRepository
{
    ValueTask<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task InsertAsync (User user, CancellationToken cancellationToken = default);
    void Update (User user);
    void Delete (User user);
    
    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}