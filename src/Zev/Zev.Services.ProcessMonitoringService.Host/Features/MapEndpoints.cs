using Microsoft.AspNetCore.Mvc;

namespace Zev.Services.ProcessMonitoringService.Host.Features;

public static class MapEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
       
        
        return app;
    }

    private static async Task<IResult> HelloWorld()
        => Results.Ok("Hello, world!");
}