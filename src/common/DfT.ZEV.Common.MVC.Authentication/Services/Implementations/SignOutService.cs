using DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Common.MVC.Authentication.Services.Implementations;

public class SignOutService : ISignOutService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignOutService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignOut()
    {
        _httpContextAccessor.HttpContext.Session.Clear();
        // Sign user out
    }
}
