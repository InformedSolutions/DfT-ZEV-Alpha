using DfT.ZEV.Core.Domain.Processes.Models;

namespace DfT.ZEV.Core.Domain.Processes.Services;

/// <summary>
/// Represents a repository for managing processes.
/// </summary>
public interface IProcessRepository
{
    /// <summary>
    /// Retrieves a process by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the process.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation with the retrieved process or null if not found.</returns>
    public Task<Process?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paged collection of processes asynchronously.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of processes per page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation with a paged collection of processes.</returns>
    public Task<IEnumerable<Process>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new process asynchronously.
    /// </summary>
    /// <param name="process">The process to add.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task AddAsync(Process process, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing process.
    /// </summary>
    /// <param name="process">The process to update.</param>
    public void Update(Process process);

    /// <summary>
    /// Deletes an existing process.
    /// </summary>
    /// <param name="process">The process to delete.</param>
    public void Delete(Process process);
}
