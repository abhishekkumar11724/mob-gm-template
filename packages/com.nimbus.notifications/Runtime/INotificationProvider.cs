using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Notifications;

public interface INotificationProvider
{
    event Action<NotificationData> OnNotificationOpened;

    string Name { get; }

    bool IsInitialized { get; }

    UniTask InitializeAsync(CancellationToken cancellationToken);

    void Schedule(string id, string title, string body, DateTime fireTime, NotificationOptions? options = null);

    void Cancel(string id);

    void CancelAll();
}
