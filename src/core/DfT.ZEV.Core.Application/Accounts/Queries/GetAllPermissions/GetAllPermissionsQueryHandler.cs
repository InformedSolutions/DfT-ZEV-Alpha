using AutoMapper;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Queries.GetAllPermissions;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPermissionsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<PermissionDto>>(await _unitOfWork.Permissions.GetAllAsync(cancellationToken));
}