namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class InternalManufacturerActivity
{
    public Guid Id { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public string Status { get; private set; }
    public string ActivityType { get; private set; }
    public DateTimeOffset ActionInitiated { get; private set; }

    public InternalManufacturerActivity() { }
}