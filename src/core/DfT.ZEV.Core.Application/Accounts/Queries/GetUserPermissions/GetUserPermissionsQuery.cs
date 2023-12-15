using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Queries.GetUserPermissions;

public class GetUserPermissionsQuery : IRequest<GetUserPermissionsQueryDto>
{
  public Guid UserId { get; set; }
  public Guid ManufacturerId { get; set; }

  public GetUserPermissionsQuery(Guid userId, Guid manufacturerId)
  {
    UserId = userId;
    ManufacturerId = manufacturerId;
  }
}
