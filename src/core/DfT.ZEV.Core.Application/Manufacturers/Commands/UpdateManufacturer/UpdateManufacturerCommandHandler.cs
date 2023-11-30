using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerCommandHandler : IRequestHandler<UpdateManufacturerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateManufacturerCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.Id, cancellationToken);
        
        if (manufacturer is null)
            throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.Id);
        
        manufacturer.WithName(request.Data.Name ?? manufacturer.Name);
        manufacturer.WithPoolMemberId(request.Data.PoolMemberId ?? manufacturer.PoolMemberId);
        manufacturer.WithCo2Target(request.Data.Co2Target ?? manufacturer.Co2Target);
        manufacturer.WithDerogationStatus(request.Data.DerogationStatus ?? manufacturer.DerogationStatus);
        
        _unitOfWork.Manufacturers.Update(manufacturer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}