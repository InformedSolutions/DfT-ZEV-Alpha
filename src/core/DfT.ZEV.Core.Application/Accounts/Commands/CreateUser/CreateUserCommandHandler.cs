using System.Security.Cryptography;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Application.Accounts.Exceptions;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Models;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityPlatform _identityPlatform;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IIdentityPlatform identityPlatform, ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _identityPlatform = identityPlatform;
        _logger = logger;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.ManufacturerId, cancellationToken);
        if (manufacturer is null)
            throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.ManufacturerId);

        var permissions = (await _unitOfWork.Permissions.GetByIdsAsync(request.PermissionIds, cancellationToken)).ToList();
        var nonExistentPermissionIds = request.PermissionIds.Except(permissions.Select(x => x.Id)).ToArray();
        if (nonExistentPermissionIds.Any())
            throw UserHandlerExceptions.PermissionsNotFound(nonExistentPermissionIds);

        var id = Guid.NewGuid();
        var args = new UserRecordArgs
        {
            Email = request.Email,
            EmailVerified = false,
            Password = CreateSecureRandomString(),
            Disabled = false,
            Uid = id.ToString()
        };

        try
        {
            await _identityPlatform.CreateUser(args);
        }
        catch(Exception e)
        {
            _logger.LogError("Failed to create user: {Message}", e.Message);
            throw UserHandlerExceptions.CouldNotCreateUser(e.Message);
        }

        var user = new User(id); 
        user.UpdatePermissions(manufacturer,permissions);
        
        await _unitOfWork.Users.InsertAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created user with email {Email}", request.Email);

        return new CreateUserCommandResponse
        {
            Id = user.Id
        };
    }

    private static string CreateSecureRandomString(int count = 64) =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(count));
}