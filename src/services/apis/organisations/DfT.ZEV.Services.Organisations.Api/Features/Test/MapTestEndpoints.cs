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