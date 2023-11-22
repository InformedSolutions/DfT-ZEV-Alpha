using Zev.Core.Domain.Processes.Models;

namespace Zev.Core.Domain.Processes.Services;

public interface IProcessRepository
{
    public Task<Process?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Process>> GetPagedAsync(int page,int pageSize,CancellationToken cancellationToken = default);
    public Task AddAsync(Process process, CancellationToken cancellationToken = default);
    public void Update(Process process);
    public void Delete(Process process);
}