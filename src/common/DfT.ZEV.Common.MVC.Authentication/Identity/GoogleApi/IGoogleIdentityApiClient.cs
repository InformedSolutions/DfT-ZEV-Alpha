namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public interface IGoogleIndetityApiClient
{
    public Task<AuthorizationResponse> Authorize(string mail, string password, string tenantId);
}