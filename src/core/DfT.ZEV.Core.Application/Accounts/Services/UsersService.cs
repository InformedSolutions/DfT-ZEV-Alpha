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
    public async Task UpdateUserClaimsAsync(User user, string tenantId)
    {
        var claims = new Dictionary<string, object>();
        var mappedPermissions = user.ManufacturerBridges.Select(x
            => new { Id = x.Manufacturer.Id, Permissions = x.Permissions.Select(p => new { p.Id }) })
            .ToList();
        claims.Add("permissions", mappedPermissions);
        await _identityPlatform.SetUserClaimsAsync(user.Id, claims, tenantId);
    }

    /// <inheritdoc/>
    public async Task RequestPasswordResetAsync(User user, string hostAddress, string tenantId)
    {
        var code = await _identityPlatform.GetPasswordResetToken(user.Id, tenantId);
        var link = $"{hostAddress}/account/set-initial-password/{code}";

        _logger.LogInformation("Generate password reset link for user {Id}: {ResetLink}", user.Id, link);
    }
}