using Zev.Services.SchemeData.Api.Features.Processes.DeleteProcess;
using Zev.Services.SchemeData.Api.Features.Processes.GetProcessById;
using Zev.Services.SchemeData.Api.Features.Processes.GetProcesses;

namespace Zev.Services.SchemeData.Api.Features.Processes;

public static class MapProcessesEndpointsExtensions
{
    public static WebApplication MapProcessesEndpoints(this WebApplication app)
    {
        app.MapGet("/{processId:guid}", GetProcessByIdHandler.HandleAsync);
        app.MapGet("/", GetProcessesHandler.HandleAsync);
        app.MapDelete("/{processId:guid}", DeleteProcessHandler.HandleAsync);

        return app;
    }
}