using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand : IRequest<CreateManufacturerCommandResponse>
{
    public string Name { get; set; } = null!;
    public float Co2Target { get; set; }
    public char DerogationStatus { get; set; } 
}