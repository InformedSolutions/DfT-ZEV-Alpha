namespace DfT.ZEV.Common.Configuration;

public class TenancyConfiguration
{
    public string Admin { get; set; } = null!;
    public string Manufacturers { get; set; } = null!;
    public string AppTenant { get; set; } = null!;
}

public class TokenConfiguration
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;

}

public class NotificationQueue
{
    public string Name { get; set; }
    public string HandlerUrl { get; set; }

}

public class Queues
{
    public NotificationQueue Notification { get; set; }
}

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