using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using DfT.ZEV.Common.MVC.Authentication.Services.Interfaces;

namespace Informed.Common.Auth.Areas.Auth.Services;

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
