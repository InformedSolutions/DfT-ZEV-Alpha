namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class RolesBridge
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public Guid RoleId { get; private set; }

    public User Account { get; private set; }
    public Manufacturer Manufacturer { get; private set; }
    public Role Role { get; private set; }
    
    public RolesBridge() { }

    public RolesBridge(User account, Manufacturer manufacturer, Role role)
    {
        Id = Guid.NewGuid();
        AccountId = account.Id;
        Account = account;
        ManufacturerId = manufacturer.Id;
        Manufacturer = manufacturer;
        RoleId = role.Id;
        Role = role;
    }
}