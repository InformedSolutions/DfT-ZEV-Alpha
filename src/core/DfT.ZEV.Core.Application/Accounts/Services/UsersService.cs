using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application.Accounts.Services;


/// <inheritdoc/>
internal sealed class UsersService : IUsersService
{
    private readonly ILogger<UsersService> _logger;
    private readonly GoogleAccountApiClient _accountApi;
    public UsersService(ILogger<UsersService> logger, GoogleAccountApiClient accountApi)
    {
        _logger = logger;
        _accountApi = accountApi;
    }

    /// <inheritdoc/>
    public async Task UpdateUserClaimsAsync(User user, string tenantId)
    {
        var claims = new Dictionary<string, object>();
        var mappedPermissions = user.ManufacturerBridges.Select(x
            => new { Id = x.Manufacturer.Id, Permissions = x.Permissions.Select(p => new { p.Id }) })
            .ToList();
        claims.Add("permissions", mappedPermissions);
        
        //await _identityPlatform.SetUserClaimsAsync(user.Id, claims, tenantId);
        await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(tenantId)
            .SetCustomUserClaimsAsync(user.Id.ToString(), claims);
    }

    /// <inheritdoc/>
    public async Task RequestPasswordResetAsync(string email, string hostAddress, string tenantId)
    {
        var code = await _accountApi.GetPasswordResetToken(new GetPasswordResetTokenRequest()
        {
            Email = email,
            TenantId = tenantId
        });
        var link = $"{hostAddress}/account/set-initial-password/{code}";

        _logger.LogInformation("Generate password reset link for user {Id}: {ResetLink}", email, link);
    }
}