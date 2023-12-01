using AutoMapper;
using DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;
using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Application.Accounts.Maps;

internal class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetAllUsersDto>()
            .ForMember(x => x.Manufacturies, opt => opt
                .MapFrom(src => src.ManufacturerBridges));
        
        CreateMap<UserManufacturerBridge, ManufacturiesWithPermissions>()
            .ForMember(x => x.Name, opt => opt
                    .MapFrom(src => src.Manufacturer.Name))
            .ForMember(x => x.Id, opt => opt
                    .MapFrom(src => src.Manufacturer.Id))
            .ForMember(x => x.Permissions, opt => opt
                    .MapFrom(src => src.Permissions))
            ;

        CreateMap<Permission, ManufacturyPermissions>()
            .ForMember(x => x.Name, opt => opt
                    .MapFrom(src => src.PermissionName));
    }
}