using Zev.Services.ProcessMonitoringService.Host.Features.DeleteProcess;
using Zev.Services.ProcessMonitoringService.Host.Features.GetProcessById;
using Zev.Services.ProcessMonitoringService.Host.Features.GetProcesses;

namespace Zev.Services.ProcessMonitoringService.Host.Features;

public static class MapEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
       app.MapGet("/{processId}", () => GetProcessByIdHandler.HandleAsync);
       app.MapGet("/", GetProcessesHandler.HandleAsync);
       app.MapDelete("/{processId}", DeleteProcessHandler.HandleAsync);
       
       return app;
    }
}