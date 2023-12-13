using System.Security.Claims;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.Extensions;

public static class ClaimsPrincipalExtension
{
    public static IdentityAccountDetails GetAccountDetails(this ClaimsPrincipal principal)
    {
        var claims = principal.Claims.ToList();
        if (claims.Any())
        {
            return new IdentityAccountDetails()
            {
                Name = claims.FirstOrDefault(x => x.Type == "name")?.Value,
                Email = claims.FirstOrDefault(x => x.Type == "email")?.Value,
            };
        }

        return null;
    }
    
    
}