using DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.SchemeData.Api.Features.Vehicles;

public static class MapVehicleEndpointsExtension
{
  private const string VehiclesPath = "/vehicles/";

  public static WebApplication MapVehicleEndpoints(this WebApplication app)
  {
    app.MapGet(VehiclesPath, GetVehiclesByManufacturerId)
        .WithTags("Vehicles");

    return app;
  }

  private static async Task<IResult> GetVehiclesByManufacturerId([FromQuery] Guid manufacturerId,
      [FromServices] IMediator mediator, [FromQuery] int page = 0, [FromQuery] int pageSize = 100, CancellationToken cancellationToken = default)
      => Results.Ok(await mediator.Send(new GetVehiclesByManufacturerIdQuery(manufacturerId, page, pageSize), cancellationToken));
}