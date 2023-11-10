using System;
using Zev.Core.Infrastructure.Settings;

namespace Zev.Services.ComplianceCalculationService.Handler.Extensions;

public class EnvironmentExtensions
{
    
    /// This method retrieves the PostgresSettings from the environment variables.
    public static PostgresSettings GetPostgresSettings() =>
        new PostgresSettings
        {
            DbName = GetPostgresVariable(nameof(PostgresSettings.DbName)),
            Host = GetPostgresVariable(nameof(PostgresSettings.Host)),
            MaxPoolSize = GetPostgresVariable(nameof(PostgresSettings.MaxPoolSize)),
            Port = GetPostgresVariable(nameof(PostgresSettings.Port)),
            UseSsl = GetPostgresVariable(nameof(PostgresSettings.UseSsl)).Equals("true", StringComparison.OrdinalIgnoreCase),
            User = GetPostgresVariable(nameof(PostgresSettings.User)),
            Password = GetPostgresVariable(nameof(PostgresSettings.Password))
        };

    /// This method retrieves the SslSettings from the environment variables.
    public static SslSettings GetSslSettings() =>
        new SslSettings
        {
            PGSSLCERT = Environment.GetEnvironmentVariable(nameof(SslSettings.PGSSLCERT)) ?? string.Empty,
            PGSSLKEY = Environment.GetEnvironmentVariable(nameof(SslSettings.PGSSLKEY)) ?? string.Empty
        };
    
    private static string GetPostgresVariable(string name) => 
        Environment.GetEnvironmentVariable($"{PostgresSettings.SectionName}__{name}");
}