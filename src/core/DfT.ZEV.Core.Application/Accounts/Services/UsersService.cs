using DfT.ZEV.Common.Configuration.GoogleCloud;
using DfT.ZEV.Common.Models;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Infrastructure.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Core.Application.Accounts.Services;


/// <inheritdoc/>
internal sealed class UsersService : IUsersService
{
    private readonly IIdentityPlatform _identityPlatform;
    private readonly ILogger<UsersService> _logger;
    private readonly IOptions<GoogleCloudConfiguration> _options;
    private readonly INotificationService _notificationService;
    public UsersService(IIdentityPlatform identityPlatform, ILogger<UsersService> logger, IOptions<GoogleCloudConfiguration> options, INotificationService notificationService)
    {
        _identityPlatform = identityPlatform;
        _logger = logger;
        _options = options;
        _notificationService = notificationService;
    }

    /// <inheritdoc/>
    public async Task UpdateUserClaimsAsync(User user, string tenantId)
    {
        var claims = new Dictionary<string, object>();
        var mappedPermissions = user.ManufacturerBridges.Select(x
            => new { Id = x.Manufacturer.Id, Permissions = x.Permissions.Select(p => new { p.Id }) })
            .ToList();
        claims.Add("permissions", mappedPermissions);
        await _identityPlatform.SetUserClaimsAsync(user.Id, claims, tenantId);
    }

    /// <inheritdoc/>
    public async Task RequestPasswordResetAsync(User user, string email,string hostAddress, string tenantId)
    {
        var code = await _identityPlatform.GetPasswordResetToken(user.Id, tenantId);
        var link = $"{hostAddress}/account/set-initial-password/{code}";
        var templateId = Guid.Parse(_options.Value.Queues.Notification.PasswordResetTemplateId);
        
        
        var notification = new Notification
        {
            //Recipients = new List<string>{email},
            Recipients = new List<string>{"james.cruddas@informed.com"},
            TemplateId = templateId,
            TemplateParameters = new Dictionary<string, string>{{"password_reset_link",link}}
        };
        await _notificationService.SendNotificationAsync(notification);
        _logger.LogInformation("Generate password reset link for user {Id}: {ResetLink}", user.Id, link);
    }
}