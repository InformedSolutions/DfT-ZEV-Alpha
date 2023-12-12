using DfT.ZEV.Common.HttpClients;
using DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerIdQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application;

/// <summary>
/// Represents a client for the Scheme Data API.
/// </summary>
public class SchemeDataApiClient : BaseHttpClient
{

  /// <summary>
  /// Initializes a new instance of the <see cref="SchemeDataApiClient"/> class.
  /// </summary>
  /// <param name="httpClient">The HTTP client.</param>
  /// <param name="logger">The logger.</param>
  /// <param name="httpContextAccessor">The HTTP context accessor.</param>
  public SchemeDataApiClient(
    HttpClient httpClient,
    ILogger<BaseHttpClient> logger,
    IHttpContextAccessor httpContextAccessor
    ) : base(httpClient, logger, httpContextAccessor)
  { }

  /// <summary>
  /// Gets the vehicles by manufacturer ID asynchronously.
  /// </summary>
  /// <param name="manufacturerId">The manufacturer's identifier.</param>
  /// <param name="pageNumber">The page number.</param>
  /// <param name="pageSize">The page size.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the vehicles.</returns>
  public async Task<GetVehiclesByManufacturerIdQueryDto> GetVehiclesByManufacturerIdAsync(Guid manufacturerId, int pageNumber, int pageSize)
    => await GetAsync<GetVehiclesByManufacturerIdQueryDto>($"vehicles?manufacturerId={manufacturerId}&pageNumber={pageNumber}&pageSize={pageSize}");

}
