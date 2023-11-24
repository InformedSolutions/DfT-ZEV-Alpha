using System.Collections.Generic;

namespace DfT.ZEV.Common.ContentSecurityPolicy;

/// <summary>
/// Configuration representation for modelling content-security policies in request headers.
/// </summary>
public class CspOption
{
    private readonly string _name;
    private readonly List<string> _sources = new ();

    public CspOption(string name)
    {
        _name = name;
    }

    public CspOption AllowSelf() => Allow("'self'");

    public CspOption AllowNone() => Allow("none");

    public CspOption AllowAny() => Allow("*");

    public CspOption AllowUnsafeInline() => Allow("'unsafe-inline'");

    public CspOption Allow(string source)
    {
        _sources.Add(source);
        return this;
    }

    public override string ToString()
    {
        return _sources.Count > 0 ? $"{_name} {string.Join(" ", _sources)}; " : string.Empty;
    }
}
