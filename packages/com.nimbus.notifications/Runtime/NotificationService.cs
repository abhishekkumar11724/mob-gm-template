using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Notifications;

public sealed class NotificationService : INotificationService
{
    private readonly List<INotificationProvider> _providers = new();
    private bool _isInitialized;
    private CancellationTokenSource? _cts;

    public event Action<NotificationData>? OnNotificationOpened;

    public void AddProvider(INotificationProvider provider)
    {
        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        if (_providers.Contains(provider))
        {
            return;
        }

        _providers.Add(provider);
        provider.OnNotificationOpened += HandleNotificationOpened;

        if (_isInitialized && _cts != null)
        {
            provider.InitializeAsync(_cts.Token).Forget();
        }
    }

    public void RemoveProvider(INotificationProvider provider)
    {
        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        if (_providers.Remove(provider))
        {
            provider.OnNotificationOpened -= HandleNotificationOpened;
        }
    }

    public async UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        if (_isInitialized)
        {
            return;
        }

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var tasks = new List<UniTask>(_providers.Count);

        foreach (var provider in _providers)
        {
            if (!provider.IsInitialized)
            {
                tasks.Add(provider.InitializeAsync(_cts.Token));
            }
        }

        await UniTask.WhenAll(tasks);
        _isInitialized = true;
    }

    public void Schedule(string id, string title, string body, DateTime fireTime, NotificationOptions? options = null)
    {
        foreach (var provider in _providers)
        {
            if (provider.IsInitialized)
            {
                provider.Schedule(id, title, body, fireTime, options);
            }
        }
    }

    public void Cancel(string id)
    {
        foreach (var provider in _providers)
        {
            if (provider.IsInitialized)
            {
                provider.Cancel(id);
            }
        }
    }

    public void CancelAll()
    {
        foreach (var provider in _providers)
        {
            if (provider.IsInitialized)
            {
                provider.CancelAll();
            }
        }
    }

    private void HandleNotificationOpened(NotificationData data)
    {
        OnNotificationOpened?.Invoke(data);
    }
}
