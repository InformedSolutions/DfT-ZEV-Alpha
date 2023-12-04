using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Services;

/// <summary>
/// Provides methods for managing user-related operations.
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Updates the claims of the specified user.
    /// </summary>
    /// <param name="user">The user whose claims are to be updated.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task UpdateUserClaimsAsync(User user);
    
    /// <summary>
    /// Sends a password reset request for the specified user.
    /// </summary>
    /// <param name="user">The user who requested the password reset.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RequestPasswordResetAsync(User user);
}