using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisations.Api.Features.Auth;

public static class MapAuthEndpointsExtensions
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/accounts/", GetAllAccounts)
            .WithTags("Accounts");
        
        
        
        return app;
    }
    
    private static async Task<IResult> GetAllAccounts([FromBody] AuthenticationRequest req,[FromServices] IIdentityPlatform identityPlatform, CancellationToken cancellationToken = default)
        => Results.Ok(await identityPlatform.AuthenticateUser(req));

}