using Microsoft.AspNetCore.Mvc;
using DfT.ZEV.Core.Infrastructure.Repositories;

namespace DfT.ZEV.Services.SchemeData.Api.Features.Processes.GetProcessById;

public class GetProcessByIdHandler
{
    public static async Task<IResult> HandleAsync([FromServices] IUnitOfWork unitOfWork, [FromRoute] Guid processId,
        CancellationToken cancellationToken = default)
    {
        var process = await unitOfWork.Processes.GetByIdAsync(processId, cancellationToken);
        return process is null ? Results.NotFound() : Results.Ok();
    }
}