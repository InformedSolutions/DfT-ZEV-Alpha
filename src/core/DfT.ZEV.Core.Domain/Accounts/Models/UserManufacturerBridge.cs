using DfT.ZEV.Core.Domain.Manufacturers.Models;
// ReSharper disable UnusedMember.Local

namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class UserManufacturerBridge
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    
    public User User { get; private set; } = null!;
    public Manufacturer Manufacturer { get; private set; } = null!;

    public ICollection<Permission> Permissions { get; private set; } = null!;

    private UserManufacturerBridge() {}
    
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