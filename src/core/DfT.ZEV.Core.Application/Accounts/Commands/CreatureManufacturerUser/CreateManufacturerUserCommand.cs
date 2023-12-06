using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateManufacturerUser;

public class CreateManufacturerUserCommand : IRequest<CreateManufacturerUserCommandResponse>
{
    public string Email { get; set; } = null!;
    public Guid ManufacturerId { get; set; }
    public Guid[] PermissionIds { get; set; } = null!;
}