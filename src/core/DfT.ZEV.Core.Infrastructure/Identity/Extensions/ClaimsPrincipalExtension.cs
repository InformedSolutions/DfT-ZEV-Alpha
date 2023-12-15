using System.Security.Claims;

namespace DfT.ZEV.Core.Infrastructure.Identity.Extensions;

/// <summary>
/// Retrieves the account details from the claims principal.
/// </summary>
/// <param name="principal">The claims principal.</param>
/// <returns>An instance of <see cref="IdentityAccountDetails"/> containing the account details, or null if no claims are present.</returns>
public static class ClaimsPrincipalExtension
{
    /// <summary>
    /// Retrieves the account details from the claims principal.
    /// </summary>
    /// <param name="principal">The claims principal.</param>
    /// <returns>An instance of <see cref="IdentityAccountDetails"/> containing the account details, or null if no claims are present.</returns>
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