using DfT.ZEV.Core.Application.Accounts.Queries.GetAllPermissions;
using DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisation.Api.Features.Permissions;

public static class MapPermissionsEndpointsExtensions
{
    public static WebApplication MapPermissionsEndpoints(this WebApplication app)
    {
        app.MapGet("/permissions/", GetAllAccounts)
            .WithTags("Permissions");

        return app;
    }
    
    private static async Task<IResult> GetAllAccounts([FromServices] IMediator mediator, CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(new GetAllPermissionsQuery(), cancellationToken));
}