using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DfT.ZEV.Common.Enumerations;
using DfT.ZEV.Common.Models;
using DfT.ZEV.Common.Services;
using Google.Cloud.Functions.Hosting;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;

namespace DfT.ZEV.Services.Notifications.Handler;

/// <summary>
/// Concrete implementation of notification service provider class.
/// </summary>

[FunctionsStartup(typeof(ServiceStartup))]
public class NotificationsService : INotificationsService
{
    private readonly ILogger<NotificationsService> _logger;

    private readonly INotificationClient _notificationClient;

    /// <summary>
    /// Default constructor for a notification service implementation.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/>.</param>
    /// <param name="notificationClient"><see cref="INotificationClient"/>.</param> 
    public NotificationsService(ILogger<NotificationsService> logger, INotificationClient notificationClient)
    {
        _logger = logger;
        _notificationClient = notificationClient;
    }

    /// <inheritdoc/>
    public async Task<HttpStatusCode> SendNotification(Notification notification)
    {
        try
        {
            Dictionary<string, dynamic> templateParameters = null;

            if (notification.TemplateParameters != null)
            {
                _logger.LogInformation("Reading template parameters from request");
                templateParameters = notification.TemplateParameters.ToDictionary(k => k.Key, k => (dynamic) k.Value);
            }

            if (notification.NotificationType == NotificationType.EMAIL)
            {
                _logger.LogInformation("Handling email dispatch request");
                return SendEmailNotification(notification, templateParameters);
            }

            if (notification.NotificationType == NotificationType.SMS)
            {
                _logger.LogInformation("Handling SMS dispatch request");
                return SendSmsNotification(notification, templateParameters);
            }

            throw new ValidationException($"Unrecognised notification type {notification.NotificationType}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    /// <summary>
    /// Private helper for dispatching notifications of type email.
    /// </summary>
    /// <param name="notification"><see cref="Notification"/>.</param>
    /// <param name="templateParameters">Parameters to inject into message template placeholders.</param>
    /// <returns>Http status code indicator as to whether the dispatch attempt was successful.</returns>
    private HttpStatusCode SendEmailNotification(Notification notification, Dictionary<string, dynamic> templateParameters)
    {
        foreach (var recipient in notification.Recipients)
        {
            _logger.LogInformation($"Dispatching email using template id {notification.TemplateId}");
            var notificationResponse = _notificationClient.SendEmail(recipient, notification.TemplateId.ToString(), templateParameters);
            _logger.LogInformation($"Successfully sent email notification with ID: {notificationResponse.id}");
        }

        return HttpStatusCode.OK;
    }

    /// <summary>
    /// Private helper for dispatching notifications of type SMS.
    /// </summary>
    /// <param name="notification"><see cref="Notification"/>.</param>
    /// <param name="templateParameters">Parameters to inject into message template placeholders.</param>
    /// <returns>Http status code indicator as to whether the dispatch attempt was successful.</returns>
    private HttpStatusCode SendSmsNotification(Notification notification, Dictionary<string, dynamic> templateParameters)
    {
        foreach (var recipient in notification.Recipients)
        {
            _logger.LogInformation($"Dispatching SMS using template id {notification.TemplateId}");
            var notificationResponse = _notificationClient.SendSms(recipient, notification.TemplateId.ToString(), templateParameters);
            _logger.LogInformation($"Successfully sent SMS notification with ID: {notificationResponse.id}");
        }

        return HttpStatusCode.OK;
    }
}