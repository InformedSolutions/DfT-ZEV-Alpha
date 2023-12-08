using DfT.ZEV.Core.Application.Accounts.Queries.GetAllPermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisations.Api.Features.Permissions;

public static class MapPermissionsEndpointsExtensions
{
    private const string PermissionsPath = "/permissions/";

    public static WebApplication MapPermissionsEndpoints(this WebApplication app)
    {
        app.MapGet(PermissionsPath, GetAllPermissions)
            .WithTags("Permissions");

        return app;
    }

    private static async Task<IResult> GetAllPermissions([FromServices] IMediator mediator, CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(new GetAllPermissionsQuery(), cancellationToken));
}