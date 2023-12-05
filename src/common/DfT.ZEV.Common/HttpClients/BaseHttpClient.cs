using DfT.ZEV.Common.Exceptions;
using DfT.ZEV.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace DfT.ZEV.Common.HttpClients;

/// <summary>
/// Base class for API HTTP Client.
/// </summary>
public abstract class BaseHttpClient
{
    private const string CorrelationIdHeaderKey = "X-Correlation-Id";
    private const string HttpClientRetryMessage = "HTTP call to {AbsoluteUri} failed. Requesting a retry after attempt {NextAttemptNumber} of {MaximumAttempts}";
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfigurationRoot _configuration;
    private readonly int _httpClientRetryIntervalInMs;
    private readonly int _maximumHttpClientRequestAttempts;

    protected BaseHttpClient(HttpClient httpClient, ILogger<BaseHttpClient> logger, IHttpContextAccessor httpContextAccessor)
        : this(httpClient, logger)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected BaseHttpClient(HttpClient httpClient, ILogger<BaseHttpClient> logger)
        : this(httpClient)
    {
        Logger = logger;
    }

    protected BaseHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;

        // Build configuration to query whether a custom retry value has been assigned for failed HTTP calls.
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        _maximumHttpClientRequestAttempts = _configuration.GetValue<int?>("HttpClientMaximumRequestAttempts") ?? 5;
        _httpClientRetryIntervalInMs = _configuration.GetValue<int?>("HttpClientRequestRetryIntervalInMs") ?? 5000;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }

    protected ILogger Logger { get; private set; }

    protected HttpClient HttpClient { get; private set; }

    /// <summary>
    /// Sends GET request.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse> GetAsync<TResponse>(string uri)
    {
        return await RequestAsync<TResponse>(HttpMethod.Get, uri, false);
    }

    /// <summary>
    /// Sends GET request with query parameters.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payloadForQueryString">Object that should be translated into
    /// query parameters key-value pairs. It support also collections.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse> GetAsync<TResponse>(string uri, object payloadForQueryString)
    {
        var queryString = GetQueryString(payloadForQueryString);

        return await GetAsync<TResponse>($"{uri}?{queryString}");
    }

    /// <summary>
    /// Sends GET request. Allows NotFound response.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<HttpClientResponse<TResponse>> GetAsyncWithCode<TResponse>(string uri)
    {
        var responseMessage = await RequestAsync(HttpMethod.Get, uri, true);
        var deserializedResponse = await DeserializeResponse<TResponse>(responseMessage);

        return new HttpClientResponse<TResponse>(deserializedResponse, responseMessage.StatusCode);
    }

    /// <summary>
    /// Sends DELETE request.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse> DeleteAsync<TResponse>(string uri)
    {
        return await RequestAsync<TResponse>(HttpMethod.Delete, uri, false);
    }

    /// <summary>
    /// Sends DELETE request.
    /// </summary>
    /// <param name="uri">Request URL.</param>
    /// <returns>HTTP response message object.</returns>
    protected async Task<HttpResponseMessage> DeleteAsync(string uri)
    {
        return await RequestAsync(HttpMethod.Delete, uri, false);
    }

    /// <summary>
    /// Sends POST request.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <typeparam name="TRequest">Type of request payload.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payload">Request payload.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse> PostAsync<TResponse, TRequest>(string uri, TRequest payload)
    {
        return await RequestAsync<TResponse, TRequest>(HttpMethod.Post, uri, payload, false);
    }

    /// <summary>
    /// Sends POST request.
    /// </summary>
    /// <typeparam name="TRequest">Type of request payload.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payload">Request payload.</param>
    /// <returns>HTTP response message object.</returns>
    protected async Task<HttpResponseMessage> PostAsync<TRequest>(string uri, TRequest payload)
    {
        return await RequestAsync(HttpMethod.Post, uri, payload, false);
    }

    /// <summary>
    /// Sends POST request.
    /// </summary>
    /// <param name="uri">Request URL.</param>
    /// <returns>HTTP response message object.</returns>
    protected async Task<HttpResponseMessage> PostAsync(string uri)
    {
        return await RequestAsync(HttpMethod.Post, uri, new { }, false);
    }

    /// <summary>
    /// Sends POST request.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <typeparam name="TRequest">Type of request payload.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payload">Request payload.</param>
    /// <returns>Object containing deserialized response and response status code.</returns>
    protected async Task<HttpClientResponse<TResponse>> PostAsyncWithCode<TResponse, TRequest>(string uri, TRequest payload)
    {
        var responseMessage = await RequestAsync(HttpMethod.Post, uri, payload, true);
        var deserializedResponse = await DeserializeResponse<TResponse>(responseMessage);

        return new HttpClientResponse<TResponse>(deserializedResponse, responseMessage.StatusCode);
    }

    /// <summary>
    /// Sends PATCH request.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <typeparam name="TRequest">Type of request payload.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payload">Request payload.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse> PatchAsync<TResponse, TRequest>(string uri, TRequest payload)
    {
        return await RequestAsync<TResponse, TRequest>(HttpMethod.Patch, uri, payload, false);
    }

    /// <summary>
    /// Sends PATCH request.
    /// </summary>
    /// <typeparam name="TRequest">Type of request payload.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payload">Request payload.</param>
    /// <returns>HTTP response message object.</returns>
    protected async Task<HttpResponseMessage> PatchAsync<TRequest>(string uri, TRequest payload)
    {
        return await RequestAsync(HttpMethod.Patch, uri, payload, false);
    }

    /// <summary>
    /// Sends PUT request.
    /// </summary>
    /// <typeparam name="TResponse">Type of request response.</typeparam>
    /// <typeparam name="TRequest">Type of request payload.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payload">Request payload.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse> PutAsync<TResponse, TRequest>(string uri, TRequest payload)
    {
        return await RequestAsync<TResponse, TRequest>(HttpMethod.Put, uri, payload, false);
    }

    /// <summary>
    /// Sends PUT request.
    /// </summary>
    /// <typeparam name="TRequest">Type of request payload.</typeparam>
    /// <param name="uri">Request URL.</param>
    /// <param name="payload">Request payload.</param>
    /// <returns>HTTP response message object.</returns>
    protected async Task<HttpResponseMessage> PutAsync<TRequest>(string uri, TRequest payload)
    {
        return await RequestAsync(HttpMethod.Put, uri, payload, false);
    }

    /// <summary>
    /// Handles failed response.
    /// Can be overridden if different action is required.
    /// For example if NotFound status should not be treated like an error.
    /// </summary>
    /// <param name="httpResponseMessage">HTTP response message.</param>
    /// <exception cref="NetworkIssuesException">Throws exception by default.</exception>
    protected virtual void HandleFailedResponse(HttpResponseMessage httpResponseMessage)
    {
        throw new NetworkIssuesException(httpResponseMessage.StatusCode);
    }

    private async Task<TResponse> RequestAsync<TResponse>(HttpMethod httpMethod, string uri, bool allowNotOk)
    {
        var requestMessage = new HttpRequestMessage(httpMethod, uri);
        var responseMessage = await SendRequestAsync(requestMessage, allowNotOk);
        if (allowNotOk && responseMessage.StatusCode != HttpStatusCode.OK)
        {
            return default;
        }

        return await DeserializeResponse<TResponse>(responseMessage);
    }

    private async Task<HttpResponseMessage> RequestAsync(HttpMethod httpMethod, string uri, bool allowNotOk)
    {
        var requestMessage = new HttpRequestMessage(httpMethod, uri);
        return await SendRequestAsync(requestMessage, allowNotOk);
    }

    private async Task<TResponse> RequestAsync<TResponse, TRequest>(HttpMethod httpMethod, string uri, TRequest payload, bool allowNotOk)
    {
        var responseMessage = await RequestAsync(httpMethod, uri, payload, allowNotOk);
        if (allowNotOk && responseMessage.StatusCode != HttpStatusCode.OK)
        {
            return default;
        }

        return await DeserializeResponse<TResponse>(responseMessage);
    }

    private async Task<HttpResponseMessage> RequestAsync<TRequest>(HttpMethod httpMethod, string uri, TRequest payload, bool allowNotOk)
    {
        var serializedPayload = JsonSerializer.Serialize(payload, _jsonOptions);
        var requestMessage = new HttpRequestMessage(httpMethod, uri)
        {
            Content = new StringContent(serializedPayload, Encoding.UTF8, "application/json"),
        };

        Logger.LogDebug("Sending HTTP request with serialized payload value: {SerializedPayload}", serializedPayload);
        return await SendRequestAsync(requestMessage, allowNotOk);
    }

    private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, bool allowNotOk)
    {
        var originalMessage = requestMessage;
        HttpResponseMessage response;

        InjectCorrelationId(requestMessage);

        Logger?.LogDebug(
            "Sending HTTP request with correlation ID: {CorrelationId}",
            requestMessage.Headers.GetValues("X-Correlation-ID").First());

        // Attempt calls up to the maximum configured threshold.
        for (int currentHttpClientRequestAttempt = 1; currentHttpClientRequestAttempt <= _maximumHttpClientRequestAttempts; currentHttpClientRequestAttempt++)
        {
            try
            {
                response = await HttpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (allowNotOk)
                {
                  return response;
                }
            }
            catch (Exception ex)
            {
                Logger?.LogDebug(ex, "Internal network issues detected. Task Failed with an exception.");

                if (currentHttpClientRequestAttempt <= _maximumHttpClientRequestAttempts)
                {
                    Logger?.LogDebug(
                        HttpClientRetryMessage,
                        requestMessage.RequestUri?.AbsoluteUri,
                        currentHttpClientRequestAttempt,
                        _maximumHttpClientRequestAttempts);

                    await Task.Delay(_httpClientRetryIntervalInMs * currentHttpClientRequestAttempt);

                    // Note the request must be cloned as the same message cannot be sent twice.
                    requestMessage = await HttpRequestMessageHelper.CloneHttpRequestMessage(originalMessage);

                    continue;
                }

                throw new NetworkIssuesException(ex);
            }

            if (currentHttpClientRequestAttempt < _maximumHttpClientRequestAttempts)
            {
                Logger?.LogDebug(
                    HttpClientRetryMessage,
                    requestMessage.RequestUri?.AbsoluteUri,
                    currentHttpClientRequestAttempt,
                    _maximumHttpClientRequestAttempts);

                await Task.Delay(_httpClientRetryIntervalInMs * currentHttpClientRequestAttempt);

                // Note the request must be cloned as the same message cannot be sent twice.
                requestMessage = await HttpRequestMessageHelper.CloneHttpRequestMessage(requestMessage);
            }
            else
            {
                Logger?.LogError(
                    "Internal network issues detected: {HttpStatusCode}, {ReasonPhrase}, {AbsoluteUri}",
                    response.StatusCode,
                    response.ReasonPhrase,
                    requestMessage.RequestUri?.AbsoluteUri);

                HandleFailedResponse(response);
            }
        }

        Logger?.LogError(
                "HTTP call to {AbsoluteUri} failed after {MaximumAttempts} attempts",
                requestMessage.RequestUri?.AbsoluteUri,
                _maximumHttpClientRequestAttempts);

        throw new NetworkIssuesException();
    }

    private async Task<TResponse> DeserializeResponse<TResponse>(HttpResponseMessage httpResponseMessage)
    {
        var dataString = await httpResponseMessage.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(dataString))
        {
            return default;
        }

        try
        {
            var result = JsonSerializer.Deserialize<TResponse>(dataString, _jsonOptions);
            return result;
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Cannot deserialize response as {TResponse}.", typeof(TResponse).ToString());
            throw;
        }
    }

    private string GetQueryString(object obj)
    {
        var queryParameters = new List<string>();
        var properties = obj.GetType().GetProperties().Where(item => item.GetValue(obj, null) != null);
        foreach (var property in properties)
        {
            var value = property.GetValue(obj, null);
            if (value is ICollection collection)
            {
                foreach (var currentValue in collection)
                {
                    var encodedValue = HttpUtility.UrlEncode(currentValue?.ToString());
                    queryParameters.Add($"{property.Name}={encodedValue}");
                }
            }
            else
            {
                var encodedValue = HttpUtility.UrlEncode(value.ToString());
                queryParameters.Add($"{property.Name}={encodedValue}");
            }
        }

        return string.Join("&", queryParameters.ToArray());
    }

    /// <summary>
    /// Private helper to inject correlation IDs using the inbound http context (if available).
    /// </summary>
    /// <param name="requestMessage">Request message to be dispatched by the HttpClient.</param>
    private void InjectCorrelationId(HttpRequestMessage requestMessage)
    {
        if (_httpContextAccessor?.HttpContext != null)
        {
            if (!requestMessage.Headers.Contains(CorrelationIdHeaderKey))
            {
                requestMessage.Headers.Add(CorrelationIdHeaderKey, _httpContextAccessor.HttpContext.Request.Headers["X-Correlation-ID"].ToString());
            }
        }
        else
        {
            // Note this is used in cases where a Lambda function is executing so no http context is available
            var correlationId = Guid.NewGuid().ToString();
            Logger?.LogInformation("Generated correlation ID: {CorrelationId}", correlationId);
            requestMessage.Headers.Add(CorrelationIdHeaderKey, correlationId);
        }
    }
}
