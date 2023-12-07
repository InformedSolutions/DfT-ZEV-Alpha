using AutoMapper;
using DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerIdQuery;
using DfT.ZEV.Core.Domain.Vehicles.Models;

namespace DfT.ZEV.Core.Application;

public class VehicleProfile : Profile
{
  public VehicleProfile()
  {
    CreateMap<Vehicle, VehicleListElementDto>();
  }
}
