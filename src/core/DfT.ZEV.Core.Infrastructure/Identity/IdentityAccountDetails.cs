namespace DfT.ZEV.Core.Infrastructure.Identity;

public class IdentityAccountDetails
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    public IEnumerable<Guid> AssignedManufacturers { get; set; }
}