using DfT.ZEV.Services.SchemeData.Api.Features.Processes.DeleteProcess;
using DfT.ZEV.Services.SchemeData.Api.Features.Processes.GetProcessById;
using DfT.ZEV.Services.SchemeData.Api.Features.Processes.GetProcesses;

namespace DfT.ZEV.Services.SchemeData.Api.Features.Processes;

public static class MapProcessesEndpointsExtensions
{
    public static WebApplication MapProcessesEndpoints(this WebApplication app)
    {
        app.MapGet("/processes/{processId:guid}", GetProcessByIdHandler.HandleAsync)
            .WithTags("Processes");
        
        app.MapGet("/processes", GetProcessesHandler.HandleAsync)
            .WithTags("Processes");

        app.MapDelete("/processes/{processId:guid}", DeleteProcessHandler.HandleAsync)
            .WithTags("Processes");

        return app;
    }
}