using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Processes.Values;

namespace DfT.ZEV.Core.Domain.Processes.Services;

/// <summary>
/// Represents a service for managing processes.
/// </summary>
public interface IProcessService
{
    /// <summary>
    /// Creates a new process asynchronously with the specified identifier and process type.
    /// </summary>
    /// <param name="id">The unique identifier of the process.</param>
    /// <param name="processType">The type of the process to create.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with the created process.</returns>
    ValueTask<Process> CreateProcessAsync(Guid id, ProcessType processType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts an existing process asynchronously with the specified identifier and optional metadata.
    /// </summary>
    /// <param name="id">The unique identifier of the process to start.</param>
    /// <param name="metadata">Optional metadata associated with starting the process.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with the started process.</returns>
    ValueTask<Process> StartProcessAsync(Guid id, object? metadata = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finishes an existing process asynchronously with the specified identifier and optional result.
    /// </summary>
    /// <param name="id">The unique identifier of the process to finish.</param>
    /// <param name="result">Optional result associated with finishing the process.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with the finished process.</returns>
    ValueTask<Process> FinishProcessAsync(Guid id, object? result = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fails an existing process asynchronously with the specified identifier and optional result.
    /// </summary>
    /// <param name="id">The unique identifier of the process to fail.</param>
    /// <param name="result">Optional result associated with failing the process.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with the failed process.</returns>
    ValueTask<Process> FailProcessAsync(Guid id, object? result = default, CancellationToken cancellationToken = default);
}
