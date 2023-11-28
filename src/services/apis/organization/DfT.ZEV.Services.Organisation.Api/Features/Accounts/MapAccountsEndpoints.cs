using DfT.ZEV.Services.Organisation.Api.Features.Accounts.GetAll;

namespace DfT.ZEV.Services.Organisation.Api.Features.Accounts;

public static class MapAccountsEndpointsExtension
{
    public static WebApplication MapAccountsEndpoints(this WebApplication app)
    {
        app.MapGet("/accounts/", GetAllAccountsHandler.HandleAsync)
            .WithTags("Accounts");

        return app;
    }
}