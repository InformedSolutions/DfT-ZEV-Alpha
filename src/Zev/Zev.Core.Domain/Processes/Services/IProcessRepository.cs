using Zev.Core.Domain.Processes.Models;

namespace Zev.Core.Domain.Processes.Services;

public interface IProcessRepository
{
    public Task<Process?> GetProcessByIdAsync(Guid id, CancellationToken ct = default);
    public Task AddProcessAsync(Process process, CancellationToken ct = default);
    public Task UpdateProcessAsync(Process process, CancellationToken ct = default);
    public Task<IEnumerable<Process>> GetProcessesAsync(CancellationToken ct = default);
}