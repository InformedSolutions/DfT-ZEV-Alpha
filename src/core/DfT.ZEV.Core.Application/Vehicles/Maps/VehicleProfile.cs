using AutoMapper;
using DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerId;
using DfT.ZEV.Core.Domain.Vehicles.Models;

namespace DfT.ZEV.Core.Application.Vehicles.Maps;

public class VehicleProfile : Profile
{
  public VehicleProfile()
  {
    CreateMap<Vehicle, VehicleListElementDto>();
  }
}
