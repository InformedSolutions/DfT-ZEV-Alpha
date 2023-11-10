namespace Zev.Core.Infrastructure.Settings;

public class PostgresSettings
{
    public const string SectionName = "Postgres";

    public string DbName { get; set; } = null!;
    public string Host { get; set; } = null!;
    public string MaxPoolSize { get; set; } = null!;
    public string Port { get; set; } = null!;
    public bool UseSsl { get; set; }
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public string ConnectionString => 
        $"Host={Host};Port={Port};Database={DbName};Username={User};Password={Password};Maximum Pool Size={MaxPoolSize};SSL Mode={(UseSsl ? "Require" : "Disable")};Trust Server Certificate=true";
}