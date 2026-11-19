using System;
using System.Collections.Generic;
using Nimbus.Core;

namespace Nimbus.Attribution;

public sealed class AttributionService : IAttributionService
{
    private readonly List<IDeepLinkHandler> _handlers = new();
    private IAttributionProvider? _provider;
    private string? _userId;

    public AttributionData? AttributionData { get; private set; }

    public event Action<string>? OnDeepLinkReceived;

    public void SetProvider(IAttributionProvider provider)
    {
        if (_provider != null)
        {
            _provider.OnAttributionReceived -= HandleAttributionReceived;
            _provider.OnDeepLinkReceived -= HandleDeepLinkReceived;
        }

        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _provider.OnAttributionReceived += HandleAttributionReceived;
        _provider.OnDeepLinkReceived += HandleDeepLinkReceived;

        if (_userId != null)
        {
            _provider.SetUserId(_userId);
        }
    }

    public void RegisterHandler(IDeepLinkHandler handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        if (!_handlers.Contains(handler))
        {
            _handlers.Add(handler);
        }
    }

    public void UnregisterHandler(IDeepLinkHandler handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        _handlers.Remove(handler);
    }

    public void SetUserId(string userId)
    {
        _userId = userId;
        _provider?.SetUserId(userId);
    }

    private void HandleAttributionReceived(AttributionData data)
    {
        AttributionData = data;
        EventBus.Publish(new InstallAttributed(data));
    }

    private void HandleDeepLinkReceived(string deepLink)
    {
        bool handled = false;

        foreach (var handler in _handlers)
        {
            if (handler.CanHandle(deepLink))
            {
                handler.Handle(deepLink);
                handled = true;
                break;
            }
        }

        OnDeepLinkReceived?.Invoke(deepLink);
        EventBus.Publish(new DeepLinkReceivedEvent(deepLink, handled));
    }
}
