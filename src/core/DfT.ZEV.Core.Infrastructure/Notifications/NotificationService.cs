using DfT.ZEV.Common.Configuration.GoogleCloud;
using DfT.ZEV.Common.Models;
using Google.Cloud.Tasks.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OidcToken = Google.Cloud.Tasks.V2.OidcToken;
using Task = System.Threading.Tasks.Task;

namespace DfT.ZEV.Core.Infrastructure.Notifications;

public class NotificationService : INotificationService
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

        var token = new OidcToken
        {
            ServiceAccountEmail = _options.Value.ServiceAccount,
            Audience = _options.Value.Queues.Notification.HandlerUrl
        };
        var payload = JsonConvert.SerializeObject(notification);

        var parentQueue = new QueueName(_options.Value.ProjectId, _options.Value.Location, _options.Value.Queues.Notification.Name);

        var response = await client.CreateTaskAsync(new CreateTaskRequest
        {
            Parent = parentQueue.ToString(),
            Task = new Google.Cloud.Tasks.V2.Task
            {
                HttpRequest = new Google.Cloud.Tasks.V2.HttpRequest
                {
                    HttpMethod = Google.Cloud.Tasks.V2.HttpMethod.Post,
                    Url = _options.Value.Queues.Notification.HandlerUrl,
                    Body = ByteString.CopyFromUtf8(payload),
                    OidcToken = token
                },
                ScheduleTime = Timestamp.FromDateTime(
                    DateTime.UtcNow.AddSeconds(5))

            }
        });

        _logger.LogInformation($"Created Task {response.Name}");
    }

   
}