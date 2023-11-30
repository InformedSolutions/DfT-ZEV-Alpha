using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommand : IRequest
{
    public Guid Id { get; set; }
    
    public DeleteManufacturerCommand(Guid id) => Id = id;
}