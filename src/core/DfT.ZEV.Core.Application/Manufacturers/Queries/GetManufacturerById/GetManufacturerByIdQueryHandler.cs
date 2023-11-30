using AutoMapper;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;

public class GetManufacturerByIdQueryHandler : IRequestHandler<GetManufacturerByIdQuery, GetManufacturerByIdQueryDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetManufacturerByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetManufacturerByIdQueryDto> Handle(GetManufacturerByIdQuery request, CancellationToken cancellationToken)
    {
        var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.Id, cancellationToken);

        if (manufacturer is null)
            throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.Id);
        
        return _mapper.Map<GetManufacturerByIdQueryDto>(manufacturer);
    }
}