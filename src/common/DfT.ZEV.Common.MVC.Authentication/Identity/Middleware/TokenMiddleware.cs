using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DfT.ZEV.Common.MVC.Authentication.Identity.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.Middleware;

public class TokenMiddleware : IMiddleware
{
    private readonly ILogger<TokenMiddleware> _logger;
    private readonly IIdentityPlatform _identityPlatform;

    public TokenMiddleware(ILogger<TokenMiddleware> logger, IIdentityPlatform identityPlatform)
    {
        _logger = logger;
        _identityPlatform = identityPlatform;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var tokenHeader = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(tokenHeader))
        {
            var token = ExtractTokenFromHeader(tokenHeader);

            var tokenClaims = ExtractClaimsFromToken(token).ToList();
            
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(tokenClaims, "Bearer"));

            context.User = userPrincipal;
        }

        await next(context);
    }

    private string ExtractTokenFromHeader(string tokenHeader)
    {
        return tokenHeader.Replace("Bearer ", "");
    }

    private IEnumerable<Claim> ExtractClaimsFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var tokenS = jsonToken as JwtSecurityToken;

        return tokenS.Claims;
    }
    
}