using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application.Accounts.Services;


/// <inheritdoc/>
internal sealed class UsersService : IUsersService
{
    private readonly IIdentityPlatform _identityPlatform;
    private readonly ILogger<UsersService> _logger;
    public UsersService(IIdentityPlatform identityPlatform, ILogger<UsersService> logger)
    {
        _identityPlatform = identityPlatform;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task UpdateUserClaimsAsync(User user)
    {
        var claims = new Dictionary<string, object>();
        var mappedPermissions = user.ManufacturerBridges.Select(x 
            => new {Id = x.Manufacturer.Id, Permissions = x.Permissions.Select(p => new {p.Id})})
            .ToList();
        claims.Add("permissions", mappedPermissions);
        await _identityPlatform.SetUserClaimsAsync(user.Id, claims);
    }

    /// <inheritdoc/>
    public async Task RequestPasswordResetAsync(User user)
    {
        var resetLink = await _identityPlatform.GetPasswordResetLink(user.Id);
        _logger.LogInformation("Generate password reset link for user {Id}: {ResetLink}", user.Id, resetLink);
    }
}