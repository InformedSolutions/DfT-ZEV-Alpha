using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using DfT.ZEV.Common.HttpClients;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DfT.ZEV.Core.Application.Accounts.Commands.CreateManufacturerUser;

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
        => await GetAsync<GetAllManufacturersQueryResponse>($"manufacturers?search={search}");
    
    
    public async Task<GetManufacturerByIdQueryDto?> GetManufacturerByIdAsync(Guid id)
        => await GetAsync<GetManufacturerByIdQueryDto?>($"manufacturers/{id}");
    
    
    public async Task<CreateManufacturerUserCommandResponse?> CreateUserAsync(CreateManufacturerUserCommand request)
        => await PostAsync<CreateManufacturerUserCommandResponse?, CreateManufacturerUserCommand>("accounts", request);        
}