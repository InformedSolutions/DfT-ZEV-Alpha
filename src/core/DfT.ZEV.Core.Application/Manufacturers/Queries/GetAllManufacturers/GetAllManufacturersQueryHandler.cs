using AutoMapper;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;

public class GetAllManufacturersQueryHandler : IRequestHandler<GetAllManufacturersQuery,GetAllManufacturersQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllManufacturersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetAllManufacturersQueryResponse> Handle(GetAllManufacturersQuery request, CancellationToken cancellationToken)
    {
        var manufacturers = await _unitOfWork.Manufacturers.GetAllAsync(cancellationToken);

        return new GetAllManufacturersQueryResponse
        {
            Manufacturers = _mapper.Map<IEnumerable<GetAllManufacturersDto>>(manufacturers)
        };
    }
}