using Microsoft.AspNetCore.Mvc;
using Zev.Core.Infrastructure.Repositories;

namespace Zev.Services.ProcessMonitoringService.Host.Features.GetProcessById;

public class GetProcessByIdHandler 
{
    public static async Task<IResult> HandleAsync([FromServices] IUnitOfWork unitOfWork,[FromRoute] Guid processId ,CancellationToken cancellationToken = default)
    {
        var process = await unitOfWork.Processes.GetByIdAsync(processId, cancellationToken);
        return process is null ? Results.NotFound() : Results.Ok();
    }
    
}