using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;

public abstract class GoogleApiClientBase
{
    protected static string SerialiseToCamelCaseJson(object input)
    {
        return JsonConvert.SerializeObject(input, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
    
    protected static string SerialiseToSnakeCaseJson(object input)
    {
        return JsonConvert.SerializeObject(input, new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        });
    }
}