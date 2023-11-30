using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;

public class CreateUserCommand : IRequest<CreateUserCommandResponse>
{
    public string Email { get; set; } = null!;
    public Guid ManufacturerId { get; set; }
    public Guid[] PermissionIds { get; set; } = null!;
}