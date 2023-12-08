using DfT.ZEV.Core.Application;
using DfT.ZEV.Core.Application.Accounts.Commands.CreateManufacturerUser;
using DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisations.Api.Features.Accounts;

public static class MapAccountsEndpointsExtension
{
    public static WebApplication MapAccountsEndpoints(this WebApplication app)
    {
        app.MapGet("/accounts/", GetAllAccounts)
            .WithTags("Accounts");

        app.MapPost("/accounts/", CreateManufacturerAccount)
            .WithTags("Accounts");

        app.MapGet("/accounts/{id}/permissions", GetUserPermissionsForManufacturer)
            .WithTags("Accounts");

        return app;
    }

    private static async Task<IResult> GetAllAccounts([FromServices] IMediator mediator, CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(new GetAllUsersQuery(), cancellationToken));

    private static async Task<IResult> CreateManufacturerAccount([FromBody] CreateManufacturerUserCommand req, [FromServices] IMediator mediator, CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(req, cancellationToken));

    private static async Task<IResult> GetUserPermissionsForManufacturer([FromRoute] Guid id, [FromQuery] Guid manufacturerId, [FromServices] IMediator mediator, CancellationToken cancellationToken = default)
    => Results.Ok(await mediator.Send(new GetUserPermissionsQuery(id, manufacturerId), cancellationToken));
}