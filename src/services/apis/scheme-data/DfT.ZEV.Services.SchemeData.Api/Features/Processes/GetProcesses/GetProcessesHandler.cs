using Microsoft.AspNetCore.Mvc;
using DfT.ZEV.Core.Infrastructure.Repositories;

namespace DfT.ZEV.Services.SchemeData.Api.Features.Processes.GetProcesses;

public class GetProcessesHandler
{
    public static async Task<IResult> HandleAsync([FromServices] IUnitOfWork unitOfWork, [FromQuery] int page = 0,
        [FromQuery] int pageSize = 100, CancellationToken cancellationToken = default)
    {
        var processes = await unitOfWork.Processes.GetPagedAsync(page, pageSize, cancellationToken);
        return Results.Ok(processes);
    }
}