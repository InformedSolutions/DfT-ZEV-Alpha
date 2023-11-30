using DfT.ZEV.Common.MVC.Authentication.Models;
using DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DfT.ZEV.Common.MVC.Authentication.Services.Implementations;

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
