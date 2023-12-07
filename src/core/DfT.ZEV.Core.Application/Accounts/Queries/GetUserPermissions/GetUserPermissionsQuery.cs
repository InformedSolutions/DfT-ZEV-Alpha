using MediatR;

namespace DfT.ZEV.Core.Application;

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
