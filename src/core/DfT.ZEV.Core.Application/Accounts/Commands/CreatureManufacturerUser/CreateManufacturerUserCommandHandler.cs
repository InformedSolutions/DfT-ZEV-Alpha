using System.Security.Cryptography;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.Configuration.GoogleCloud;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Application.Accounts.Exceptions;
using DfT.ZEV.Core.Application.Manufacturers.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Domain.Manufacturers.Models;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Core.Application.Accounts.Commands.CreateManufacturerUser;

/// <summary>
/// Handles the creation of a new user.
/// </summary>
/// <remarks>
/// This class is responsible for handling a <see cref="CreateManufacturerUserCommand"/> and returning a <see cref="CreateManufacturerUserCommandResponse"/>.
/// It uses an instance of <see cref="IUnitOfWork"/> to interact with the database,
/// an instance of <see cref="IIdentityPlatform"/> to interact with the identity platform,
/// and an instance of <see cref="IUsersService"/> to manage user-related operations.
/// </remarks>
public class CreateManufacturerUserCommandHandler : IRequestHandler<CreateManufacturerUserCommand, CreateManufacturerUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityPlatform _identityPlatform;
    private readonly IOptions<GoogleCloudConfiguration> _googleCloudConfiguration;
    private readonly IOptions<ServicesConfiguration> _serviceConfiguration;
    private readonly IUsersService _usersService;
    private readonly ILogger<CreateManufacturerUserCommandHandler> _logger;
    public CreateManufacturerUserCommandHandler(IOptions<GoogleCloudConfiguration> googleCloudConfiguration, IOptions<ServicesConfiguration> serviceConfiguration, IUnitOfWork unitOfWork, IIdentityPlatform identityPlatform, ILogger<CreateManufacturerUserCommandHandler> logger, IUsersService usersService)
    {
        _unitOfWork = unitOfWork;
        _identityPlatform = identityPlatform;
        _logger = logger;
        _usersService = usersService;
        _googleCloudConfiguration = googleCloudConfiguration;
        _serviceConfiguration = serviceConfiguration;
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
    public async Task<CreateManufacturerUserCommandResponse> Handle(CreateManufacturerUserCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.ManufacturerId, cancellationToken);
        if (manufacturer is null)
            throw ManufacturerHandlerExceptions.ManufacturerNotFound(request.ManufacturerId);

        var permissions = (await _unitOfWork.Permissions.GetByIdsAsync(request.PermissionIds, cancellationToken)).ToList();
        var nonExistentPermissionIds = request.PermissionIds.Except(permissions.Select(x => x.Id)).ToArray();
        if (nonExistentPermissionIds.Any())
            throw UserHandlerExceptions.PermissionsNotFound(nonExistentPermissionIds);

        if (request.PermissionIds.Length == 0)
            permissions = (await _unitOfWork.Permissions.GetAllAsync(cancellationToken)).ToList();

        var id = Guid.NewGuid();
        var password = CreateSecureRandomString();
        var tenant = _googleCloudConfiguration.Value.Tenancy.Manufacturers;
        var args = new UserRecordArgs
        {
            Email = request.Email,
            EmailVerified = true,
            Password = password,
            Disabled = false,
            Uid = id.ToString()
        };

        try
        {
            await _identityPlatform.CreateUser(args, tenant);
            await CreateUser(args, tenant, id, manufacturer, permissions, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to create user: {Message}", e.Message);
            throw UserHandlerExceptions.CouldNotCreateUser(e.Message);
        }

        _logger.LogInformation("Created user with email {Email}", request.Email);

        return new CreateManufacturerUserCommandResponse(id);
    }

    private async Task CreateUser(UserRecordArgs args, string tenant, Guid id, Manufacturer manufacturer, List<Permission> permissions, CancellationToken cancellationToken)
    {
        try
        {
            var user = new User(id);
            user.UpdatePermissions(manufacturer, permissions);

            await _unitOfWork.Users.InsertAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _usersService.UpdateUserClaimsAsync(user, tenant);

            _logger.LogInformation("Created user with email {Email}", args.Email);
            await _usersService.RequestPasswordResetAsync(user, _serviceConfiguration.Value.ManufacturerPortalBaseUrl, tenant);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to create user: {Message}", ex.Message);
            await _identityPlatform.DeleteUserAsync(id, tenant);
            throw UserHandlerExceptions.CouldNotCreateUser(ex.Message);

        }
    }

    private static string CreateSecureRandomString(int count = 64) =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(count));
}