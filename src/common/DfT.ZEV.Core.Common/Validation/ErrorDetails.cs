using System.Text.Json;

namespace DfT.ZEV.Common.Validation;

/// <summary>
/// Class definition for modelling a comment error messaging shape (typically used at API layer).
/// </summary>
public class ErrorDetails
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}