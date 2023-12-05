using System.Diagnostics;
using System.Net.Http.Json;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;
using DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AuthorizationRequest = DfT.ZEV.Common.MVC.Authentication.Identity.AuthorizationRequest;

namespace DfT.ZEV.Common.Services.Clients;

public class OrganizationApiClient
{
    private readonly HttpClient _httpClient;
    
    public OrganizationApiClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<GetAllManufacturersQueryResponse?> GetManufacturersAsync(string search)
    {
        var response = await _httpClient.GetAsync($"manufacturers?search={search}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var res =  JsonConvert.DeserializeObject<GetAllManufacturersQueryResponse>(content); 
            return res;
        }
        return null;
    }
    
    public async Task<GetManufacturerByIdQueryDto?> GetManufacturerByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"manufacturers/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var res =  JsonConvert.DeserializeObject<GetManufacturerByIdQueryDto>(content); 
            return res;
        }
        return null;
    }
    
    public async Task<CreateUserCommandResponse?> CreateUserAsync(CreateUserCommand request)
    {
        var response = await _httpClient.PostAsJsonAsync("accounts", request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var res =  JsonConvert.DeserializeObject<CreateUserCommandResponse>(content); 
            return res;
        }
        return null;
    }
    
    // public async Task<AuthorizationResponse> AuthorizeUser(AuthorizationRequest request)
    // {
    //     var response = await _httpClient.PostAsJsonAsync("accounts", new AuthorizationRequest(email, password));
    //     if (response.IsSuccessStatusCode)
    //     {
    //         var content = await response.Content.ReadAsStringAsync();
    //         var res =  JsonConvert.DeserializeObject<AuthorizationResponse>(content); 
    //         return res;
    //     }
    //     return null;
    // }
}