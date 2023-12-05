using System.Net.Http.Json;
using DfT.ZEV.Common.Middlewares.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DfT.ZEV.Core.Application.Clients;

public class BaseApiServiceClient
{
    protected readonly HttpClient _httpClient;

    public BaseApiServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected static async Task<TResponse> HandleResponse<TResponse>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonConvert.DeserializeObject<BaseErrorResponse>(content);
            throw new ApiClientException(errorResponse.Data.Message);
        }   
        
        var res =  JsonConvert.DeserializeObject<TResponse>(content); 
        return res;
    }
}