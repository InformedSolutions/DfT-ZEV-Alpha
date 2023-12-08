using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace DfT.ZEV.Common.MVC.Authentication.HealthChecks;

public static class HealthcheckExtensions
{
    /// <summary>
    /// Configures the application to use health checks with a specific path and response writer.
    /// </summary>
    /// <param name="app">The application to configure.</param>
    /// <returns>The configured application.</returns>
    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }

    /// <summary>
    /// Configures the application to use health checks with a specific path and response writer in an MVC context.
    /// </summary>
    /// <param name="app">The application to configure.</param>
    /// <returns>The configured application builder.</returns>
    public static IApplicationBuilder UseHealthChecksMvc(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}