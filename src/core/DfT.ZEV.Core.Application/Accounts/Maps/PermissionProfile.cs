using AutoMapper;
using DfT.ZEV.Core.Application.Accounts.Queries.GetAllPermissions;
using DfT.ZEV.Core.Application.Accounts.Queries.GetUserPermissions;
using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Application.Accounts.Maps;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        CreateMap<Permission, PermissionDto>();
        CreateMap<Permission, ManufacturerPermission>()
            .ForMember(x => x.Name, opt => opt
                .MapFrom(src => src.PermissionName));

    }
}