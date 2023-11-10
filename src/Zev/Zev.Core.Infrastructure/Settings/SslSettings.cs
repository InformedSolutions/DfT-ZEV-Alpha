namespace Zev.Core.Infrastructure.Settings;

public class SslSettings
{
    public const string SectionName = "Ssl";
    
    public string PGSSLCERT { get; set; } = null!;
    public string PGSSLKEY { get; set; } = null!;
}