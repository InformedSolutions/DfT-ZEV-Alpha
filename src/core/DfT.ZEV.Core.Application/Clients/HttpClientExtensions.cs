using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Core.Application.Clients;

internal static class HttpClientExtensions
{
    public static HttpClient WithCorrelationId(this HttpClient client,IHttpContextAccessor httpContextAccessor)
    {
        var correlationId = httpContextAccessor.HttpContext?.TraceIdentifier;
        if (!string.IsNullOrEmpty(correlationId))
        {
            client.DefaultRequestHeaders.Add("x-correlation-id", correlationId);
        }

        return client;
    }
}