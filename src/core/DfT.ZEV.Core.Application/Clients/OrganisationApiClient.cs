using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using DfT.ZEV.Common.HttpClients;
using DfT.ZEV.Core.Application.Accounts.Commands.CreatureManufacturerUser;
using DfT.ZEV.Core.Application.Accounts.Queries.GetUserPermissions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Core.Application.Clients;

/// <summary>
/// Configures the application to use health checks with a specific path and response writer in an MVC context.
/// </summary>
/// <param name="app">The application to configure.</param>
/// <returns>The configured application.</returns>
public class OrganisationApiClient : BaseHttpClient
{

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganisationApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public OrganisationApiClient(
        HttpClient httpClient,
        ILogger<OrganisationApiClient> logger,
        IHttpContextAccessor httpContextAccessor)
        : base(httpClient, logger, httpContextAccessor)
    {
    }

    /// <summary>
    /// Gets all manufacturers asynchronously.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the manufacturers.</returns>
    public async Task<GetAllManufacturersQueryResponse?> GetManufacturersAsync(string search)
        => await GetAsync<GetAllManufacturersQueryResponse>($"manufacturers?search={search}");

    /// <summary>
    /// Gets a manufacturer by ID asynchronously.
    /// </summary>
    /// <param name="id">The manufacturer's identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the manufacturer.</returns>
    public async Task<GetManufacturerByIdQueryDto?> GetManufacturerByIdAsync(Guid id)
        => await GetAsync<GetManufacturerByIdQueryDto?>($"manufacturers/{id}");

    /// <summary>
    /// Creates a user asynchronously.
    /// </summary>
    /// <param name="request">The request to create a user for specific manufacturer.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response from the user creation.</returns>
    public async Task<CreateManufacturerUserCommandResponse?> CreateManufacturerUserAsync(CreateManufacturerUserCommand request)
        => await PostAsync<CreateManufacturerUserCommandResponse?, CreateManufacturerUserCommand>("accounts", request);

    /// <summary>
    /// Gets the user permissions asynchronously.
    /// </summary>
    /// <param name="userId">The user's identifier.</param>
    /// <param name="manufacturerId">The manufacturer's identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user permissions.</returns>
    public async Task<GetUserPermissionsQueryDto> GetUserPermissionsAsync(Guid userId, Guid manufacturerId)
        => await GetAsync<GetUserPermissionsQueryDto>($"accounts/{userId}/permissions?manufacturerId={manufacturerId}");
}