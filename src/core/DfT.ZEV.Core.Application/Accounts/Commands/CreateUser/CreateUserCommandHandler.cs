using DfT.ZEV.Core.Application.Accounts.Exceptions;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Models;
using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Manufacturers.GetByIdAsync(request.ManufacturerId, cancellationToken) is null)
            throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.ManufacturerId);

       
        var permissions = await _unitOfWork.Permissions.GetByIdsAsync(request.PermissionIds, cancellationToken);
        permissions = permissions.ToList();
        
        var nonExistentPermissionIds = request.PermissionIds.Except(permissions.Select(x => x.Id)).ToArray();
        if (nonExistentPermissionIds.Any())
            throw UserHandlerExceptions.PermissionsNotFound(nonExistentPermissionIds);
        

        var userId = Guid.NewGuid();

        var user = new User(userId); 
        user.AddPermissions(permissions);
        
        await _unitOfWork.Users.InsertAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateUserCommandResponse
        {
            Id = userId
        };
    }
}