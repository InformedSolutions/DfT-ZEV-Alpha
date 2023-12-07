using Microsoft.AspNetCore.Mvc;
using MediatR;
using DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerIdQuery;

namespace DfT.ZEV.Services.SchemeData.Api;

public static class MapVehicleEndpointsExtension
{

  public static WebApplication MapVehicleEndpoints(this WebApplication app)
  {
    app.MapGet("/vehicles", GetVehiclesByManufacturerId)
      .WithTags("Vehicles");

    return app;
  }

  private static async Task<IResult> GetVehiclesByManufacturerId([FromQuery] Guid manufacturerId,
    [FromServices] IMediator mediator, [FromQuery] int page = 0, [FromQuery] int pageSize = 100, CancellationToken cancellationToken = default)
  {
    var res = await mediator.Send(new GetVehiclesByManufacturerIdQuery(manufacturerId, page, pageSize), cancellationToken);
    return Results.Ok(res);
  }
}
