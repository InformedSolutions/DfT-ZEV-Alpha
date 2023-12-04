using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using DfT.ZEV.Common.Enumerations;
using DfT.ZEV.Common.Models;
using DfT.ZEV.Common.Services;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;

namespace DfT.ZEV.Services.Notifications.Handler;

/// <summary>
/// Concrete implementation of notification service provider class.
/// </summary>
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
            if (notification.NotificationType == NotificationType.EMAIL)
            {
                return SendEmailNotification(notification);
            }

            if (notification.NotificationType == NotificationType.SMS)
            {
                return SendSmsNotification(notification);
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
    /// <returns>Http status code indicator as to whether the dispatch attempt was successful.</returns>
    private HttpStatusCode SendEmailNotification(Notification notification)
    {
        foreach (var recipient in notification.Recipients)
        {
            var notificationResponse = _notificationClient.SendEmail(recipient, notification.TemplateId.ToString(), notification.TemplateParameters);
            _logger.LogInformation($"Successfully sent email notification with ID: {notificationResponse.id}");
        }

        return HttpStatusCode.OK;
    }

    /// <summary>
    /// Private helper for dispatching notifications of type SMS.
    /// </summary>
    /// <param name="notification"><see cref="Notification"/>.</param>
    /// <returns>Http status code indicator as to whether the dispatch attempt was successful.</returns>
    private HttpStatusCode SendSmsNotification(Notification notification)
    {
        foreach (var recipient in notification.Recipients)
        {
            var notificationResponse = _notificationClient.SendSms(recipient, notification.TemplateId.ToString(), notification.TemplateParameters);
            _logger.LogInformation($"Successfully sent SMS notification with ID: {notificationResponse.id}");
        }

        return HttpStatusCode.OK;
    }
}