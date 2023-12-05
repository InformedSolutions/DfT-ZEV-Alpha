using System.Net;

namespace DfT.ZEV.Common.HttpClients;

/// <summary>
/// Generic class for modelling an HTTP response.
/// </summary>
/// <typeparam name="T">The type of response object expected.</typeparam>
public class HttpClientResponse<T>
{
    public HttpClientResponse(T response, HttpStatusCode statusCode)
    {
        Response = response;
        StatusCode = statusCode;
    }

    public T Response { get; }

    public HttpStatusCode StatusCode { get; }
}
