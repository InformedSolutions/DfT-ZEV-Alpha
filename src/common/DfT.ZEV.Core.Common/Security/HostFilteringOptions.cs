using System.Collections.Generic;

namespace DfT.ZEV.Common.Security;

/// <summary>
/// A configuration class for allowed host filtering settings.
/// </summary>
public class HostFilteringOptions
{
    public string ErrorRoute { get; set; }

    public List<string> AllowedHostnames { get; set; }

    public List<string> ExcludedPaths { get; set; }
}
