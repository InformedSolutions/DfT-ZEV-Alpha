using Microsoft.AspNetCore.Mvc;
using Zev.Core.Infrastructure.Repositories;

namespace Zev.Services.ProcessMonitoringService.Host.Features.GetProcessById;

public class GetProcessByIdHandler 
{
    public static async Task<IResult> HandleAsync([FromServices] IUnitOfWork unitOfWork,[FromRoute] Guid id ,CancellationToken cancellationToken = default)
    {
        var process = await unitOfWork.Processes.GetByIdAsync(id, cancellationToken);
        return process is null ? Results.NotFound() : Results.Ok();
    }
    
}