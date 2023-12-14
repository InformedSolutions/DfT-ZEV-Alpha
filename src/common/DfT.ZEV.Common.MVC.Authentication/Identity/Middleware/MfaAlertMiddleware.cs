using System.Security.Claims;
using DfT.ZEV.Common.MVC.Authentication.Identity.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.Middleware;

public class MfaAlertMiddleware : IMiddleware
{
    private readonly IHttpContextAccessor _contextAccessor;
    private IIdentityPlatform _identityPlatform;

    public MfaAlertMiddleware(IHttpContextAccessor contextAccessor, IIdentityPlatform identityPlatform)
    {
        _contextAccessor = contextAccessor;
        _identityPlatform = identityPlatform;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Claims.Any())
        {
            var email = context.User.Claims.First(c => c.Type == "email").Value;
            var tenant = ExtractTenantFromClaims(context.User.Claims);
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var userInfo = await _identityPlatform.LookupUser(token, email, tenant);

            var userHasMfaEnabled = userInfo.Users[0].MfaInfo?.Any() ?? false;

            if (!userHasMfaEnabled && !context.Request.Path.Value.EndsWith("/account/mfa-not-enabled"))
            {
                context.Response.Redirect("/account/mfa-not-enabled");
                return;
            }
        }

        await next(context);
    }

    private string ExtractTenantFromClaims(IEnumerable<Claim> claims)
    {
        var firebaseClaims = claims
            .Where(c => c.Type == "firebase")
            .Select(c => c.Value)
            .FirstOrDefault();

        var firebaseJson = System.Text.Json.JsonDocument.Parse(firebaseClaims).RootElement;

        return firebaseJson.GetProperty("tenant").GetString();
    }
}