namespace DfT.ZEV.Common.MVC.Authentication.Identity;

public class IdentityAccountDetails
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    public IEnumerable<Guid> AssignedManufacturers { get; set; }
}