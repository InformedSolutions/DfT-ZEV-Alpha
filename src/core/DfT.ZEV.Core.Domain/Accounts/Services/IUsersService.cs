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
    /// <param name="tenantId">The identity tenant in which a user can be located.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateUserClaimsAsync(User user, string tenantId);
    
    /// <summary>
    /// Sends a password reset request for the specified user.
    /// </summary>
    /// <param name="user">The user who requested the password reset.</param>
    /// <param name="hostAddress">The base URL at which a service is deployed.</param>
    /// <param name="tenantId">The identity tenant in which a user can be located.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RequestPasswordResetAsync(User user, string hostAddress, string tenantId);
}