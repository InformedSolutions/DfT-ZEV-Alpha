using FirebaseAdmin;

namespace DfT.ZEV.Core.Infrastructure.Identity;

internal sealed class IdentityPlatform : IIdentityPlatform
{
    public IdentityPlatform()
    {
        if(FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create();
        }
    }   
}