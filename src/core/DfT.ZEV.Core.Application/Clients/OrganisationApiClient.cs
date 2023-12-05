using DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using DfT.ZEV.Common.HttpClients;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Core.Application.Clients;

public class OrganisationApiClient : BaseHttpClient
{
    public OrganisationApiClient(
        HttpClient httpClient,
        ILogger<OrganisationApiClient> logger,
        IHttpContextAccessor httpContextAccessor)
        : base(httpClient, logger, httpContextAccessor)
    {
    }

    public async Task<GetAllManufacturersQueryResponse?> GetManufacturersAsync(string search)
    {
        return await GetAsync<GetAllManufacturersQueryResponse>($"manufacturers?search={search}");
    }
    
    public async Task<GetManufacturerByIdQueryDto?> GetManufacturerByIdAsync(Guid id)
    {
        return await GetAsync<GetManufacturerByIdQueryDto?>($"manufacturers/{id}");
    }
    
    public async Task<CreateUserCommandResponse?> CreateUserAsync(CreateUserCommand request)
    {
        return await PostAsync<CreateUserCommandResponse?, CreateUserCommand>("accounts", request);        
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