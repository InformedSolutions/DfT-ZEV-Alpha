using DfT.ZEV.Common.Models;

namespace DfT.ZEV.Common.Notifications;

public interface INotificationService
{
    Task SendNotificationAsync(Notification notification, CancellationToken ct = default);
}