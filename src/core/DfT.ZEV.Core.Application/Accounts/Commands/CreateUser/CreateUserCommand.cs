using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;

public class CreateUserCommand : IRequest<CreateUserCommandResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}