using DfT.ZEV.Core.Domain.Common;
using DfT.ZEV.Core.Domain.Manufacturers.Models;

namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class User : IAggregateRoot
{
    public Guid Id { get;  }
    public DateTimeOffset CreatedAt { get; }
    
    public ICollection<ManufacturerPool> ManufacturerPools { get; private set; } = new List<ManufacturerPool>();
    public ICollection<UserManufacturerBridge> ManufacturerBridges { get; private set; } = new List<UserManufacturerBridge>();
    
    public User() { }
    
    public User(Guid id)
    {
        Id = id;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void UpdatePermissions(Manufacturer manufacturer, IEnumerable<Permission> permissions)
    {
        var bridge = ManufacturerBridges.FirstOrDefault(x => x.ManufacturerId == manufacturer.Id);

        if (bridge == null)
        {
            bridge = new UserManufacturerBridge(this, manufacturer);
            ManufacturerBridges.Add(bridge);
        }

        bridge.SetPermissions(permissions);
    }
}