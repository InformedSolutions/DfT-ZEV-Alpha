using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommandHandler : IRequestHandler<DeleteManufacturerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteManufacturerCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.Id, cancellationToken);
        
        if (manufacturer is null)
            throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.Id);

        _unitOfWork.Manufacturers.Delete(manufacturer);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}