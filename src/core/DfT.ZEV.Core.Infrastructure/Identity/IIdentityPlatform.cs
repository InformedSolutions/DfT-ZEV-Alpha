using FirebaseAdmin.Auth;

namespace DfT.ZEV.Core.Infrastructure.Identity;

public interface IIdentityPlatform
{
    public ValueTask<UserRecord> CreateUser(UserRecordArgs userRecordArgs);
}