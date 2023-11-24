using System;
using System.Collections.Generic;
using System.Linq;

namespace DfT.ZEV.Common.ConfigurationModels;

/// <summary>
/// Configuration model for custom assets.
/// </summary>
public class CustomAssetsOptions
{
    /// <summary>
    /// Gets or sets the name of the configuration key appsettings to source custom asset settings from.
    /// </summary>
    public const string ConfigurationName = "CustomAssets";

    /// <summary>
    /// Gets or sets a value indicating whether custom styling assets are enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a URL where a logo can be sourced from for custom styling.
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// Gets or sets the alt text to display for a client logo.
    /// </summary>
    public string LogoAlt { get; set; }

    /// <summary>
    /// Gets or sets the favicon value to render in html content.
    /// </summary>
    public string Favicon { get; set; }

    /// <summary>
    /// Gets or sets a list containing the names of additional CSS stylesheets to load.
    /// </summary>
    public List<string> Css { get; set; } = new ();

    /// <summary>
    /// Gets or sets additional content security policies to add.
    /// </summary>
    public List<string> Csp { get; set; } = new ();

    /// <summary>
    /// Gets or sets helper method for returning a formatted content security policy string (used in HTTP headers) based on configuration values.
    /// </summary>
    /// <returns>A formatted content security policy string (used in HTTP headers) based on configuration values.</returns>
    public string GetCspString()
    {
        var hosts = new List<string>();

        foreach (var url in Css.Union(new List<string> { Logo, Favicon }))
        {
            if (!string.IsNullOrEmpty(url))
            {
                var host = new Uri(url).Host;
                hosts.Add(host);
            }
        }

        hosts = hosts.Distinct().Where(x => !string.IsNullOrEmpty(x)).ToList();

        return string.Join(" ", hosts);
    }
}
