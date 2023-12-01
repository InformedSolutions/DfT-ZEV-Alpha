using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;

namespace DfT.ZEV.Core.Application.Accounts.Services;

internal sealed class UsersService : IUsersService
{
    private readonly IIdentityPlatform _identityPlatform;

    public UsersService(IIdentityPlatform identityPlatform)
    {
        _identityPlatform = identityPlatform;
    }

    public async Task UpdateUserClaimsAsync(User user)
    {
        var claims = new Dictionary<string, object>();
        var mappedPermissions = user.ManufacturerBridges.Select(x => new {x.Manufacturer.Name, x.Permissions}).ToList();
        claims.Add("permissions", mappedPermissions);
        await _identityPlatform.SetUserClaimsAsync(user.Id, claims);
    }
}