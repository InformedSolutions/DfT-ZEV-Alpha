using AutoMapper;
using DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;
using DfT.ZEV.Core.Domain.Accounts.Models;

namespace DfT.ZEV.Core.Application.Accounts.Maps;

internal class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetAllUsersDTO>();
        CreateMap<Permission, UserPermissionDto>()
            .ForMember(x => x.Name, 
                opt => opt
                    .MapFrom(src => src.PermissionName));
    }
}