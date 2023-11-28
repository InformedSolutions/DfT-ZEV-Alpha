namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class RolesBridge
{
    public Guid AccountId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public Guid RoleId { get; private set; }

    public User Account { get; private set; }
    public Manufacturer Manufacturer { get; private set; }
    public Role Role { get; private set; }
    
    public RolesBridge() { }

    public RolesBridge(User account, Manufacturer manufacturer, Role role)
    {
        AccountId = account.Id;
        ManufacturerId = manufacturer.Id;
        RoleId = role.Id;
    }
}