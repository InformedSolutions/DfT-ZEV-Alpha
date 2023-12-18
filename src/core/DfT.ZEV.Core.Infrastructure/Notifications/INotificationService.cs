using DfT.ZEV.Common.Models;

namespace DfT.ZEV.Core.Infrastructure.Notifications;

public interface INotificationService
{
    Task SendNotificationAsync(Notification notification, CancellationToken ct = default);
}