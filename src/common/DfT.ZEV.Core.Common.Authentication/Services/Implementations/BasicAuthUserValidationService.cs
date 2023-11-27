using Microsoft.Extensions.Configuration;
using DfT.ZEV.Core.Common.Authentication.Models;

namespace DfT.ZEV.Core.Common.Authentication.Services;

/// <inheritdoc />
public class BasicAuthUserValidationService : IBasicAuthUserValidationService
{
    private readonly Dictionary<string, BasicAuthUser> _allowedUsers;

    public BasicAuthUserValidationService(IConfiguration configuration)
    {
        _allowedUsers = configuration
            .GetSection("BasicAuth")
            .GetSection("Users")
            .Get<List<BasicAuthUser>>()
            .ToDictionary(item => item.Username, item => item);
    }

    public bool AreCredentialsValid(string username, string password)
    {
        return _allowedUsers.TryGetValue(username, out BasicAuthUser basicAuthUser)
            && basicAuthUser.Password == password;
    }
}
