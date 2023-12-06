using System.Security.Cryptography;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Application.Accounts.Exceptions;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;

/// <summary>
/// Handles the creation of a new user.
/// </summary>
/// <remarks>
/// This class is responsible for handling a <see cref="CreateUserCommand"/> and returning a <see cref="CreateUserCommandResponse"/>.
/// It uses an instance of <see cref="IUnitOfWork"/> to interact with the database,
/// an instance of <see cref="IIdentityPlatform"/> to interact with the identity platform,
/// and an instance of <see cref="IUsersService"/> to manage user-related operations.
/// </remarks>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityPlatform _identityPlatform;
    private readonly IUsersService _usersService;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IIdentityPlatform identityPlatform, ILogger<CreateUserCommandHandler> logger, IUsersService usersService)
    {
        _unitOfWork = unitOfWork;
        _identityPlatform = identityPlatform;
        _logger = logger;
        _usersService = usersService;
    }

    /// <summary>
    /// Handles the creation of a new user.
    /// </summary>
    /// <param name="request">The request to create a new user.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response from the user creation.</returns>
    /// <exception cref="ManufacturerHandlerExceptions.ManufacturerNotFound">Thrown when the manufacturer is not found.</exception>
    /// <exception cref="UserHandlerExceptions.PermissionsNotFound">Thrown when one or more permissions are not found.</exception>
    /// <exception cref="UserHandlerExceptions.CouldNotCreateUser">Thrown when user creation failed.</exception>
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
        var password = CreateSecureRandomString();
        _logger.LogInformation("Password for user with email {Email} is {Password}", request.Email, password);
        var args = new UserRecordArgs
        {
            Email = request.Email,
            EmailVerified = false,
            Password = password,
            Disabled = false,
            Uid = id.ToString()
        };

        try
        {
            await _identityPlatform.CreateUser(args);
            var user = new User(id);
            user.UpdatePermissions(manufacturer, permissions);

            await _unitOfWork.Users.InsertAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _usersService.UpdateUserClaimsAsync(user);

            _logger.LogInformation("Created user with email {Email}", request.Email);
            await _usersService.RequestPasswordResetAsync(user);
            _logger.LogInformation("Requested password reset for user with email {Email}", request.Email);

        }
        catch (Exception e)
        {
            _logger.LogError("Failed to create user: {Message}", e.Message);
            throw UserHandlerExceptions.CouldNotCreateUser(e.Message);
        }

        _logger.LogInformation("Created user with email {Email}", request.Email);


        return new CreateUserCommandResponse(id);
    }

    private static string CreateSecureRandomString(int count = 64) =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(count));
}