namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi.Account.Requests;

public class LookupUserRequest
{
    public string IdToken { get; set; }
    public List<string> Email { get; set; }
    public string  TenantId { get; set; }
}