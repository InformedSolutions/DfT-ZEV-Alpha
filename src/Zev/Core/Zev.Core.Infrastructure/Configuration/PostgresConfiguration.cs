using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Zev.Core.Infrastructure.Configuration;

/// <summary>
/// Represents the settings required to connect to a PostgreSQL database.
/// </summary>
public class PostgresConfiguration
{
    public const string SectionName = "Postgres";

    [Required]
    public string DbName { get; set; }
    
    [Required]
    public string Host { get; set; }
    
    [Required]
    public string MaxPoolSize { get; set; }
    
    [Required]
    public string Port { get; set; }
    
    [Required]
    public bool UseSsl { get; set; }
    
    [Required]
    public string User { get; set; } 
    
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Gets the connection string for the PostgreSQL database based on included properties.
    /// </summary>
    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={DbName};Username={User};Password={Password};Maximum Pool Size={MaxPoolSize};SSL Mode={(UseSsl ? "Require" : "Disable")};Trust Server Certificate=true";
}