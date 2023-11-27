using Microsoft.AspNetCore.Mvc;
using DfT.ZEV.Core.Infrastructure.Repositories;

namespace DfT.ZEV.Services.SchemeData.Api.Features.Processes.DeleteProcess;

public class DeleteProcessHandler
{
    public static async Task<IResult> HandleAsync([FromServices] UnitOfWork unitOfWork, [FromRoute] Guid processId,
        CancellationToken cancellationToken = default)
    {
        var process = await unitOfWork.Processes.GetByIdAsync(processId, cancellationToken);
        if (process is null) return Results.NotFound();
        unitOfWork.Processes.Delete(process);
        await unitOfWork.SaveChangesAsync();
        return Results.Ok();
    }
}