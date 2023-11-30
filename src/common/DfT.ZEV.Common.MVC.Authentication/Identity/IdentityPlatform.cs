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
}