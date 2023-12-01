using FirebaseAdmin.Auth;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public interface IIdentityPlatform
{
    public ValueTask<UserRecord> CreateUser(UserRecordArgs userRecordArgs);
    public Task SetUserClaimsAsync(Guid userId, IReadOnlyDictionary<string, object> claims);
}