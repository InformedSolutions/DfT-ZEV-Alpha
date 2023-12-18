using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.Configuration.GoogleCloud;
using DfT.ZEV.Common.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Tasks.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace DfT.ZEV.Core.Infrastructure.Notifications;

internal class NotificationService : INotificationService
{
    private readonly IOptions<GoogleCloudConfiguration> _options;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IOptions<GoogleCloudConfiguration> options, ILogger<NotificationService> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task SendNotificationAsync(Notification notification, CancellationToken ct = default)
    {
        var client = await CloudTasksClient.CreateAsync(ct);

        var oidcToken = await GetOidcTokenAsync(ct);
        var payload = JsonConvert.SerializeObject(notification);

        var parentQueue = new QueueName(_options.Value.ProjectId, _options.Value.Location, _options.Value.Queues.Notification.Name);

        var response = await client.CreateTaskAsync(new CreateTaskRequest
        {
            Parent = parentQueue.ToString(),
            Task = new Google.Cloud.Tasks.V2.Task
            {
                HttpRequest = new HttpRequest
                {
                    HttpMethod = Google.Cloud.Tasks.V2.HttpMethod.Post,
                    Url = _options.Value.Queues.Notification.HandlerUrl,
                    Body = ByteString.CopyFromUtf8(payload),
                    Headers = { { "Authorization", $"Bearer {oidcToken}" } }
                },
                ScheduleTime = Timestamp.FromDateTime(
                    DateTime.UtcNow.AddSeconds(5))

            }
        });

        _logger.LogInformation($"Created Task {response.Name}");
    }

    private async Task<Google.Apis.Auth.OAuth2.OidcToken> GetOidcTokenAsync(CancellationToken ct)
    {
        var credential = await GoogleCredential.GetApplicationDefaultAsync(ct);
        var oidcTokenOptions = OidcTokenOptions.FromTargetAudience(_options.Value.Queues.Notification.HandlerUrl);
        return await credential.GetOidcTokenAsync(oidcTokenOptions, ct);
    }
}