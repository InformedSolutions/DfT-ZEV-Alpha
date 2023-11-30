using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerCommand : IRequest
{
    public Guid Id { get; set; }
    public UpdateManufacturerData Data { get; set; }
    
    public UpdateManufacturerCommand(Guid id, UpdateManufacturerData data)
    {
        Id = id;
        Data = data;
    }
}