using System.Security.Cryptography;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Application.Accounts.Exceptions;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Models;
using FirebaseAdmin.Auth;
using MediatR;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityPlatform _identityPlatform;
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IIdentityPlatform identityPlatform)
    {
        _unitOfWork = unitOfWork;
        _identityPlatform = identityPlatform;
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


        var id = Guid.NewGuid();
        var args = new UserRecordArgs()
        {
            Email = request.Email,
            EmailVerified = false,
            Password = CreateSecureRandomString(),
            Disabled = false,
            Uid = id.ToString()
        };

        var userRes = await _identityPlatform.CreateUser(args);
        
        var user = new User(id); 
        user.AddPermissions(permissions);
        
        await _unitOfWork.Users.InsertAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateUserCommandResponse
        {
            Id = user.Id
        };
    }
    
    private static string CreateSecureRandomString(int count = 64) =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(count));
}