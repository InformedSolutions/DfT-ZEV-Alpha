using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Manufacturers.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand,CreateManufacturerCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateManufacturerCommandHandler> _logger;
    
    public CreateManufacturerCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateManufacturerCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateManufacturerCommandResponse> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating manufacturer {Name}", request.Name);

        if (await _unitOfWork.Manufacturers.GetByNameAsync(request.Name, cancellationToken) is not null)
            throw ManufacturerHandlerExceptions.ManufacturerAlreadyExists(request.Name);
        
        var manufacturer = new Manufacturer(request.Name)
            .WithCo2Target(request.Co2Target)
            .WithDerogationStatus(request.DerogationStatus)
            .WithPoolMemberId(Guid.NewGuid());
        
        await _unitOfWork.Manufacturers.InsertAsync(manufacturer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Created manufacturer {Name}", request.Name);
        
        return new CreateManufacturerCommandResponse
        {
            Id = manufacturer.Id
        };
    }
}