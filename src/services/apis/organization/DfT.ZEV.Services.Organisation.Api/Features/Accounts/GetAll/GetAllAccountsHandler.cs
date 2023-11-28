using DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisation.Api.Features.Accounts.GetAll;

internal static class GetAllAccountsHandler
{
    public static async Task<IResult> HandleAsync([FromServices] IMediator mediator, CancellationToken cancellationToken = default)
    {
        return Results.Ok(await mediator.Send(new GetAllUsersQuery(), cancellationToken));
    }
}