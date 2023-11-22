using Microsoft.AspNetCore.Mvc;
using Zev.Services.ProcessMonitoringService.Host.Features.DeleteProcess;
using Zev.Services.ProcessMonitoringService.Host.Features.GetProcessById;
using Zev.Services.ProcessMonitoringService.Host.Features.GetProcesses;
using Zev.Services.ProcessMonitoringService.Host.Features.UpdateProcess;

namespace Zev.Services.ProcessMonitoringService.Host.Features;

public static class MapEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
       app.MapGet("/{processId}", () => GetProcessByIdHandler.HandleAsync);
       app.MapGet("/", GetProcessesHandler.HandleAsync);
       app.MapPost("/", UpdateProcessHandler.HandleAsync);
       app.MapDelete("/{processId}", DeleteProcessHandler.HandleAsync);
       
       return app;
    }
}