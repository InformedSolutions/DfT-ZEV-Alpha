using DfT.ZEV.Common.ConfigurationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Common.ContentSecurityPolicy;

public class ContentSecurityPolicyService
{
    private readonly CustomAssetsOptions _customAssetsOptions;
    private readonly string _additionalCspValues;
    private readonly string _cdnUrl;

    public ContentSecurityPolicyService(IOptions<CustomAssetsOptions> customAssetsOptions, IConfiguration configuration)
    {
        _customAssetsOptions = customAssetsOptions.Value;
        _additionalCspValues = configuration.GetValue<string>("AdditionalCspSourceValues");
        _cdnUrl = configuration.GetValue<string>("cdn:url");
    }

    public string GetAdditionalCspSourceValues()
    {
        var customAssetsCsp = _customAssetsOptions.GetCspString();
        return $"{_additionalCspValues} {customAssetsCsp} {_cdnUrl}";
    }
}
