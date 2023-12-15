using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DfT.ZEV.Core.Infrastructure.Identity.GoogleApi;


/// <summary>
/// Base class for Google API clients.
/// </summary>
public abstract class GoogleApiClientBase
{
    /// <summary>
    /// Serializes an object to JSON using camel case property names.
    /// </summary>
    /// <param name="input">The object to serialize.</param>
    /// <returns>The serialized JSON string.</returns>
    protected static string SerialiseToCamelCaseJson(object input)
    {
        return JsonConvert.SerializeObject(input, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }

    /// <summary>
    /// Serializes an object to JSON using snake case property names.
    /// </summary>
    /// <param name="input">The object to serialize.</param>
    /// <returns>The serialized JSON string.</returns>
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