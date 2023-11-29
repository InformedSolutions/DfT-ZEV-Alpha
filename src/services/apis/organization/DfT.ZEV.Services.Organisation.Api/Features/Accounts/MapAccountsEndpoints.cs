using DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisation.Api.Features.Accounts;

public static class MapAccountsEndpointsExtension
{
    public static WebApplication MapAccountsEndpoints(this WebApplication app)
    {
        app.MapGet("/accounts/", GetAllAccounts)
            .WithTags("Accounts");

        return app;
    }
    
    private static async Task<IResult> GetAllAccounts([FromServices] IMediator mediator, CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(new GetAllUsersQuery(), cancellationToken));

}