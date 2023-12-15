using AutoMapper;
using DfT.ZEV.Core.Application.Accounts.Exceptions;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Queries.GetUserPermissions;

public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, GetUserPermissionsQueryDto>
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper _mapper;

  public GetUserPermissionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _mapper = mapper;
  }

  public async Task<GetUserPermissionsQueryDto> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
  {
    var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
    if (user is null)
      throw UserHandlerExceptions.UserNotFound(request.UserId);

    var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.ManufacturerId, cancellationToken);
    if (manufacturer is null)
      throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.ManufacturerId);

    var bridge = user.ManufacturerBridges.FirstOrDefault(x => x.ManufacturerId == manufacturer.Id);
    var permissions = bridge?.Permissions.Select(x => new ManufacturerPermission { Id = x.Id, Name = x.PermissionName }) ?? Enumerable.Empty<ManufacturerPermission>();
    return new GetUserPermissionsQueryDto
    {
      UserId = user.Id,
      ManufacturerId = manufacturer.Id,
      ManufacturerName = manufacturer.Name,
      Permissions = permissions
    };
  }
}
