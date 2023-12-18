namespace DfT.ZEV.Common.Configuration.GoogleCloud;

public class GoogleCloudConfiguration
{
    public const string SectionName = "GoogleCloud";
    
    public string ServiceAccount {get; set; }

    public string ProjectId { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string Location { get; set; } = null!;
    public TenancyConfiguration Tenancy { get; set; } = null!;
    public TokenConfiguration Token { get; set; } = null!;
    public Queues Queues { get; set; }
}