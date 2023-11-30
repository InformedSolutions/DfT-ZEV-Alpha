using Microsoft.Extensions.DependencyInjection;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public static class IdentityExtensions
{
    public static void AddIdentityPlatform(this IServiceCollection services)
    {
        services.AddTransient<IIdentityPlatform, IdentityPlatform>();
    }
}