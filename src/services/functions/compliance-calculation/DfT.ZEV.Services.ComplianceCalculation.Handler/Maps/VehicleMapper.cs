using AutoMapper;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using DfT.ZEV.Services.ComplianceCalculation.Handler.DTO;

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.Maps;

public class VehicleMapper : Profile
{
    public VehicleMapper()
    {
        CreateMap<RawVehicleDTO, Vehicle>()
            .ForMember(x => x.Summary, opt => opt.MapFrom(src => new VehicleSummary(src.Vin)));
    }
}