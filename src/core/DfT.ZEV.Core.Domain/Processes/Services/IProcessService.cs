using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Processes.Values;

namespace DfT.ZEV.Core.Domain.Processes.Services;

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