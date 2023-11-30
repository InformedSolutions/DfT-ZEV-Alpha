using DfT.ZEV.Core.Domain.Manufacturers.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class UserManufacturerBridge
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    
    public User User { get; private set; }
    public Manufacturer Manufacturer { get; private set; }

    public ICollection<Permission> Permissions { get; private set; }
    
    protected UserManufacturerBridge() {}
    
    public UserManufacturerBridge(User user, Manufacturer manufacturer)
    {
        Id = Guid.NewGuid();
        
        UserId = user.Id;
        User = user;
        
        ManufacturerId = manufacturer.Id;
        Manufacturer = manufacturer;
    }
    
    public UserManufacturerBridge SetPermissions(IEnumerable<Permission> permissions)
    {
        Permissions = permissions.ToList();
        return this;
    }
   
}