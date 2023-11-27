using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace DfT.ZEV.Core.Infrastructure.Configuration;

/// <summary>
///     Represents the settings required to connect to a PostgreSQL database.
/// </summary>
public class PostgresConfiguration
{
    public const string SectionName = "Postgres";

    [Required] public string DbName { get; set; } = null!;

    [Required] public string Host { get; set; } = null!;

    [Required] public string MaxPoolSize { get; set; } = null!;

    [Required] public string Port { get; set; } = null!;

    [Required] public bool UseSsl { get; set; }

    [Required] public string User { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;

    /// <summary>
    ///     Gets the connection string for the PostgreSQL database based on included properties.
    /// </summary>
    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={DbName};Username={User};Password={Password};Maximum Pool Size={MaxPoolSize};SSL Mode={(UseSsl ? "Require" : "Disable")};Trust Server Certificate=true";
}