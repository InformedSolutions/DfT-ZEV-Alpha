using System.Net.Http.Json;
using DfT.ZEV.Common.MVC.Authentication.Identity.GoogleApi;
using DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DfT.ZEV.Core.Application.Clients.OrganisationApi;

internal sealed class OrganisationApiClient :BaseApiServiceClient ,IOrganisationApiClient 
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public OrganisationApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetAllManufacturersQueryResponse?> GetManufacturersAsync(string search)
    {
        var client = _httpClient.WithCorrelationId(_httpContextAccessor);
        var response = await client.GetAsync($"manufacturers?search={search}");
        return await HandleResponse<GetAllManufacturersQueryResponse>(response);
    }
    
    public async Task<GetManufacturerByIdQueryDto?> GetManufacturerByIdAsync(Guid id)
    {
        var client = _httpClient.WithCorrelationId(_httpContextAccessor);
        var response = await client.GetAsync($"manufacturers/{id}");
        return await HandleResponse<GetManufacturerByIdQueryDto>(response);
    }
    
    public async Task<CreateUserCommandResponse?> CreateUserAsync(CreateUserCommand request)
    {
        var client = _httpClient.WithCorrelationId(_httpContextAccessor);
        var response = await client.PostAsJsonAsync("accounts", request);
        return await HandleResponse<CreateUserCommandResponse>(response);
    }
    
    public async Task<AuthorizationResponse> AuthorizeUser(AuthorizationRequest request)
    {
        var client = _httpClient.WithCorrelationId(_httpContextAccessor);
        var response = await client.PostAsJsonAsync("auth", request);
        return await HandleResponse<AuthorizationResponse>(response);      
    }
}