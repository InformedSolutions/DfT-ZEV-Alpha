using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public class TokenMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var tokenHeader = context.Request.Headers["Authorization"];
        
        if (!string.IsNullOrEmpty(tokenHeader))
        {
            var token = tokenHeader.ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(tokenS.Claims, "Bearer"));
            
            context.User = userPrincipal;
        }

        await next(context);
    }
}