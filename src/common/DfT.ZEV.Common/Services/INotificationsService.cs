using System.Net;
using DfT.ZEV.Common.Models;

namespace DfT.ZEV.Common.Services;

/// <summary>
/// Interface contract for a notification service.
/// </summary>
public interface INotificationsService
{
    Task<HttpStatusCode> SendNotification(Notification notification);
}