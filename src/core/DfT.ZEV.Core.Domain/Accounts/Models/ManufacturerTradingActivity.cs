namespace DfT.ZEV.Core.Domain.Accounts.Models;

public sealed class ManufacturerTradingActivity
{
    public Guid Id { get; private set; }
    
    public char Status { get; private set; }
    
    public Guid? InitiatingManufacturerId { get; private set; }
    public Manufacturer? InitiatingManufacturer { get; private set; }
    
    public Guid? AcceptingManufacturerId { get; private set; }
    public Manufacturer? AcceptingManufacturer { get; private set; }
    
    public string ApplicableScheme { get; private set; } = null!;
    public string TradeType { get; private set; } = null!;
    public float TradeAmount { get; private set; }
    public float TradeValue { get; private set; }

    protected ManufacturerTradingActivity() { }

    public ManufacturerTradingActivity(char status, string applicableScheme, string tradeType, float tradeAmount, float tradeValue)
    {
        Id = Guid.NewGuid();
        Status = status;
        ApplicableScheme = applicableScheme;
        TradeType = tradeType;
        TradeAmount = tradeAmount;
        TradeValue = tradeValue;
    }
}