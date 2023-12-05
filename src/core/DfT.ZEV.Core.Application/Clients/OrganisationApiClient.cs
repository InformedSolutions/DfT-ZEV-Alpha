using System.Net.Http.Json;
using DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using Newtonsoft.Json;

namespace DfT.ZEV.Core.Application.Clients;

public class OrganisationApiClient
{
    private readonly HttpClient _httpClient;
    
    public OrganisationApiClient(HttpClient httpClient) => _httpClient = httpClient;

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