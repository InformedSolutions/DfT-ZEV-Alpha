namespace DfT.ZEV.ManufacturerReview.Application.Config;

/// <summary>
/// Configuration model for API connection.
/// </summary>
public class ApiConfiguration
{
    private string _baseUrl;

    /// <summary>
    /// Gets or sets API Base URL with trailing slash.
    /// </summary>
    public string BaseUrl
    {
        get => !string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith('/') ? $"{_baseUrl}/" : _baseUrl;
        set
        {
            _baseUrl = value;
        }
    }
}
