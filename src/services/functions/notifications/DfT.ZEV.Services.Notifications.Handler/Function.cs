using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DfT.ZEV.Common.Models;
using DfT.ZEV.Common.Services;
using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Services.Notifications.Handler;

/// <summary>
///     Represents an HTTP function that sends notifications (e.g. email, SMS).
/// </summary>
[FunctionsStartup(typeof(ServiceStartup))]
public class Function : IHttpFunction
{
    private readonly ILogger<Function> _logger;

    private readonly INotificationsService _notificationsService;

    public Function(ILogger<Function> logger, INotificationsService notificationsService)
    {
        _logger = logger;
        _notificationsService = notificationsService;
    }

    /// <summary>
    ///     Handles an request to send a notification.
    /// </summary>
    public async Task HandleAsync(HttpContext context)
    {
        using TextReader reader = new StreamReader(context.Request.Body);
        var json = await reader.ReadToEndAsync();
        var notification = JsonSerializer.Deserialize<Notification>(json);
        var result = _notificationsService.SendNotification(notification);
    }
}