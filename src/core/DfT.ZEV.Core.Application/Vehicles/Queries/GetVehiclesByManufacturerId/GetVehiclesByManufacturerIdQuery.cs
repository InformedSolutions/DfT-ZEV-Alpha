using MediatR;

namespace DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerIdQuery;

public class GetVehiclesByManufacturerIdQuery : IRequest<GetVehiclesByManufacturerIdQueryDto>
{
  public Guid ManufacturerId { get; set; }
  public int PageNumber { get; set; }
  public int PageSize { get; set; }

  public GetVehiclesByManufacturerIdQuery(Guid manufacturerId, int pageNumber, int pageSize)
  {
    ManufacturerId = manufacturerId;
    PageNumber = pageNumber;
    PageSize = pageSize;
  }
}