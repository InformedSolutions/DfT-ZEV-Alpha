using AutoMapper;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Manufacturers.Models;

namespace DfT.ZEV.Core.Application.Manufacturers.Maps;

public class ManufacturersProfile : Profile
{
    public ManufacturersProfile()
    {
        CreateMap<Manufacturer, GetAllManufacturersDto>();
        
        CreateMap<UserManufacturerBridge, ManufacturerUsersDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.UserId));
        CreateMap<Manufacturer, GetManufacturerByIdQueryDto>()
            .ForMember(x => x.Users, opt => opt.MapFrom(src => src.UserBridges));
    }
}