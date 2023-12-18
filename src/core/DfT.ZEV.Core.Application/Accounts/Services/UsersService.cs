using DfT.ZEV.Common.Configuration.GoogleCloud;
using DfT.ZEV.Common.Models;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Domain.Accounts.Services;
using DfT.ZEV.Core.Infrastructure.Notifications;
using Google.Cloud.Tasks.V2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

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
        
        var token = new OidcToken
        {
            ServiceAccountEmail = _options.Value.ServiceAccount,
            Audience = _options.Value.Queues.Notification.HandlerUrl
        };
        
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

/*
@ -0,0 +1,66 @@
   using DfT.ZEV.Common.Configuration;
   using Google.Apis.Auth.OAuth2;
   using Google.Cloud.Tasks.V2;
   using Google.Protobuf;
   using Google.Protobuf.WellKnownTypes;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Options;
   using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;
   using OidcToken = Google.Cloud.Tasks.V2.OidcToken;
   
   namespace DfT.ZEV.Services.Organisations.Api.Features.Test;
   
   /// <summary>
   /// This is only for test purposes
   /// </summary>
   public static class MapTestEndpointsExtensions
   {
       private const string TestPath = "/test/";
   
       public static WebApplication MapTestEndpoints(this WebApplication app)
       {
           app.MapPost(TestPath, SendTestRequest)
               .WithTags("Test");
   
           return app;
       }
   
       private static async Task<IResult> SendTestRequest([FromServices] IOptions<GoogleCloudConfiguration> _options,CancellationToken cancellationToken = default)
       { 
           var client = await CloudTasksClient.CreateAsync(cancellationToken);
           var inSeconds = 10;
   
           string payload = @"{""prop"": ""test""}";
   
         
           var options = _options.Value;
           var parentQueue = new QueueName(options.ProjectId, options.Location, options.Queues.Notification.Name);
   
           var token = new OidcToken
           {
               ServiceAccountEmail = options.ServiceAccount,
               Audience = options.Queues.Notification.HandlerUrl
           };
           
           var response = await client.CreateTaskAsync(new CreateTaskRequest
           {
               Parent = parentQueue.ToString(),
               Task = new Google.Cloud.Tasks.V2.Task
               {
                   HttpRequest = new Google.Cloud.Tasks.V2.HttpRequest
                   {
                       HttpMethod = Google.Cloud.Tasks.V2.HttpMethod.Post,
                       Url = options.Queues.Notification.HandlerUrl,
                       Body = ByteString.CopyFromUtf8(payload),
                       OidcToken = token
                   },
                   ScheduleTime = Timestamp.FromDateTime(
                       DateTime.UtcNow.AddSeconds(inSeconds)),
                   
               }
           });
   
           Console.WriteLine($"Created Task {response.Name}");
           return Results.Ok();
       }
   }
*/