using System;
using Nimbus.Core;

namespace Nimbus.Notifications;

public interface INotificationService : IService, IInitializable
{
    event Action<NotificationData> OnNotificationOpened;

    void AddProvider(INotificationProvider provider);

    void RemoveProvider(INotificationProvider provider);

    void Schedule(string id, string title, string body, DateTime fireTime, NotificationOptions? options = null);

    void Cancel(string id);

    void CancelAll();
}
