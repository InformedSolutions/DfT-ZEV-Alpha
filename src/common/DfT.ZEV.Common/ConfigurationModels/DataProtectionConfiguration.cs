namespace DfT.ZEV.Common.ConfigurationModels;

/// <summary>
/// A class for carrying .NET data protection configuration settings (see https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-7.0).
/// </summary>
public class DataProtectionConfiguration
{
    /// <summary>
    /// The key for the settings block in appsettings.
    /// </summary>
    public const string ConfigurationName = "DataProtection";

    /// <summary>
    /// Gets or sets the directory path where cryptographic keys are stored.
    /// </summary>
    public string KeyPath { get; set; }

    /// <summary>
    /// Gets or sets the application name used to segregate cryptographic keys.
    /// </summary>
    public string ApplicationName { get; set; }
}
