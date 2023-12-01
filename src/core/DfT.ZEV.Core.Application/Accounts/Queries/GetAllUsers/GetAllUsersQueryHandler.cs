using AutoMapper;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery,GetAllUsersQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);

        return new GetAllUsersQueryResponse
        {
            Users = _mapper.Map<IEnumerable<GetAllUsersDto>>(users)
        };
    }
}