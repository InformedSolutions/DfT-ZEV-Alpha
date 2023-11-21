using AutoMapper;
using Zev.Core.Domain.Vehicles;
using Zev.Services.ComplianceCalculationService.Handler.DTO;

namespace Zev.Services.ComplianceCalculationService.Handler.Maps;

public class VehicleMapper : Profile
{
    public VehicleMapper()
    {
        CreateMap<RawVehicleDTO, Vehicle>()
            .ForMember(x => x.Summary, opt => opt.MapFrom(src => new VehicleSummary(src.Vin)));
    }
}