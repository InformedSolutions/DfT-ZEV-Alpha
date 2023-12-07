using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Services;

/// <summary>
/// Deletes an existing user.
/// </summary>
/// <param name="user">The user to delete.</param>
public interface IPermissionRepository
{
    /// <summary>
    /// Retrieves all permissions.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of permissions.</returns>
    public Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves permissions by their unique identifiers.
    /// </summary>
    /// <param name="ids">The unique identifiers of the permissions.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of permissions if found; otherwise, null.</returns>
    public Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}