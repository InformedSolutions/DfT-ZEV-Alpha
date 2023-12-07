using AutoMapper;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerIdQuery;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application;

public class GetVehiclesByManufacturerIdQueryHandler : IRequestHandler<GetVehiclesByManufacturerIdQuery, GetVehiclesByManufacturerIdQueryDto>
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper _mapper;
  private readonly ILogger<GetVehiclesByManufacturerIdQueryHandler> _logger;
  public GetVehiclesByManufacturerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetVehiclesByManufacturerIdQueryHandler> logger)
  {
    _unitOfWork = unitOfWork;
    _mapper = mapper;
    _logger = logger;
  }

  public async Task<GetVehiclesByManufacturerIdQueryDto> Handle(GetVehiclesByManufacturerIdQuery request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Fetching vehicles by manufacturer id {ManufacturerId} page: {Page} pageSize: {PageSize}", request.ManufacturerId, request.PageNumber, request.PageSize);
    var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.ManufacturerId, cancellationToken);

    if (manufacturer is null)
      throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.ManufacturerId);

    var vehicles = await _unitOfWork.Vehicles.GetVehiclesByManufacturerNameAsync(manufacturer.Name, request.PageNumber, request.PageSize, cancellationToken);

    var mappedVehicles = _mapper.Map<IEnumerable<VehicleListElementDto>>(vehicles);

    return new GetVehiclesByManufacturerIdQueryDto
    {
      Vehicles = mappedVehicles,
      TotalCount = mappedVehicles.Count()
    };
  }
}
