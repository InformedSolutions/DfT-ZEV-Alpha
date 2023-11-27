using AutoMapper;
using DfT.ZEV.Core.Domain.Vehicles.Models;
using Zev.Services.ComplianceCalculation.Handler.DTO;

namespace Zev.Services.ComplianceCalculation.Handler.Maps;

public class VehicleMapper : Profile
{
    public VehicleMapper()
    {
        CreateMap<RawVehicleDTO, Vehicle>()
            .ForMember(x => x.Summary, opt => opt.MapFrom(src => new VehicleSummary(src.Vin)));
    }
}