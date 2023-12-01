using DfT.ZEV.Common.Configuration;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

internal sealed class IdentityPlatform : IIdentityPlatform
{
    private readonly IOptions<GoogleCloudConfiguration> _googleCloudConfiguration;
    public IdentityPlatform(IOptions<GoogleCloudConfiguration> googleCloudConfiguration)
    {
        _googleCloudConfiguration = googleCloudConfiguration;
        
        if(FirebaseApp.DefaultInstance == null)
        {
            var options = new AppOptions
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                ProjectId = _googleCloudConfiguration.Value.ProjectId,
            };
            FirebaseApp.Create(options);
        }
    }

    public async ValueTask<UserRecord> CreateUser(UserRecordArgs userRecordArgs)
        => await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(_googleCloudConfiguration.Value.Tenancy.Manufacturers)
            .CreateUserAsync(userRecordArgs);

    public async Task SetUserClaimsAsync(Guid userId, IReadOnlyDictionary<string, object> claims)
    {
        var user = await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(_googleCloudConfiguration.Value.Tenancy.Manufacturers)
            .GetUserAsync(userId.ToString());
        
        await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(_googleCloudConfiguration.Value.Tenancy.Manufacturers)
            .SetCustomUserClaimsAsync(user.Uid, claims);
    }

    public async Task<string> GetPasswordResetLink(Guid userId)
    {
        var user = await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(_googleCloudConfiguration.Value.Tenancy.Manufacturers)
            .GetUserAsync(userId.ToString());
        
        return await FirebaseAuth.DefaultInstance.TenantManager
            .AuthForTenant(_googleCloudConfiguration.Value.Tenancy.Manufacturers)
            .GeneratePasswordResetLinkAsync(user.Email);
    }
     
    
}