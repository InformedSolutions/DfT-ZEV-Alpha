using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateManufacturer;

public class CreateManufacturerCommand : IRequest<CreateManufacturerCommandResponse>
{
    public string Name { get; set; }
    public float Co2Target { get; set; }
    public char DerogationStatus { get; set; } 
}