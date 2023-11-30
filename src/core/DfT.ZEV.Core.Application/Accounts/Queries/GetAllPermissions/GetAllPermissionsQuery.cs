using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Queries.GetAllPermissions;

public class GetAllPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
{
    
}