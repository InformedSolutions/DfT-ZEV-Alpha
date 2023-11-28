namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class InternalManufacturerActivity
{
    public Guid Id { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public string Status { get; private set; } = null!;
    public string ActivityType { get; private set; } = null!;
    public DateTimeOffset ActionInitiated { get; private set; }

    protected InternalManufacturerActivity() { }
}