using Zev.Core.Domain.Processes.Models;
using Zev.Core.Domain.Processes.Values;

namespace Zev.Core.Domain.Processes.Services;

public interface IProcessService
{ 
    ValueTask<Process> CreateProcessAsync(Guid id, ProcessTypeEnum processType, CancellationToken cancellationToken = default);
    ValueTask<Process> StartProcessAsync(Guid id, object? metadata = default,
        CancellationToken cancellationToken = default);
    ValueTask<Process> FinishProcessAsync(Guid id, object? result = default,
        CancellationToken cancellationToken = default);
    ValueTask<Process> FailProcessAsync(Guid id, object? result = default,
        CancellationToken cancellationToken = default);
}