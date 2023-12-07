using DfT.ZEV.Common.HttpClients;
using DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerIdQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application;

public class SchemeDataApiClient : BaseHttpClient
{
  public SchemeDataApiClient(
    HttpClient httpClient,
    ILogger<BaseHttpClient> logger,
    IHttpContextAccessor httpContextAccessor
    ) : base(httpClient, logger, httpContextAccessor)
  { }

  public async Task<GetVehiclesByManufacturerIdQueryDto> GetVehiclesByManufacturerIdAsync(Guid manufacturerId, int pageNumber, int pageSize)
    => await GetAsync<GetVehiclesByManufacturerIdQueryDto>($"vehicles?manufacturerId={manufacturerId}&pageNumber={pageNumber}&pageSize={pageSize}");

}
