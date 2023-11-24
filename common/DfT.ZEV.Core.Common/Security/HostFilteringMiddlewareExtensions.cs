using Microsoft.AspNetCore.Builder;

namespace DfT.ZEV.Common.Security;

/// <summary>
/// Extension class to allow binding of host filtering middleware via Startup.cs.
/// </summary>
public static class HostFilteringMiddlewareExtensions
{
    /// <summary>
    /// Binds Hostfiltering middleware.
    /// </summary>
    /// <param name="builder">Defines a class that provides the mechanisms to configure an application's request pipeline.</param>
    /// <param name="options">Configured options for host filtering.</param>
    /// <returns>Application builder interface.</returns>
    public static IApplicationBuilder UseAllowedHostFilteringMiddleware(this IApplicationBuilder builder, HostFilteringOptions options)
    {
        return builder.UseMiddleware<HostFilteringMiddleware>(options);
    }
}
