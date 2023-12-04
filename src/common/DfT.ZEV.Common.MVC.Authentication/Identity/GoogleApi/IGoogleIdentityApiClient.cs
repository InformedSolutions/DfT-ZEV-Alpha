namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public interface IGoogleIdentityApiClient
{
    public Task<AuthorizationResponse> Authorize(string mail, string password, string tenantId);
    public Task<RefreshTokenResponse> RefreshToken(string token);

}