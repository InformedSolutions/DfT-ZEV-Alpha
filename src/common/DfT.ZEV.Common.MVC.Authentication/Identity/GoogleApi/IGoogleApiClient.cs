namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public interface IGoogleApiClient
{
    public Task<AuthorizationResponse> Authorize(string mail, string password, string tenantId);
}