using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Analytics;

public sealed class AnalyticsService : IAnalyticsService
{
    private readonly List<IAnalyticsProvider> _providers = new();
    private readonly Dictionary<string, string> _userProperties = new();
    private string? _userId;
    private bool _isInitialized;
    private CancellationTokenSource? _cts;

    public string? UserId => _userId;

    public IReadOnlyDictionary<string, string> UserProperties => _userProperties;

    public void AddProvider(IAnalyticsProvider provider)
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

        if (_isInitialized && _cts != null)
        {
            InitializeAndApplyPropertiesAsync(provider, _cts.Token).Forget();
        }
    }

    private async UniTaskVoid InitializeAndApplyPropertiesAsync(IAnalyticsProvider provider, CancellationToken cancellationToken)
    {
        if (!provider.IsInitialized)
        {
            await provider.InitializeAsync(cancellationToken);
        }

        if (_userId != null)
        {
            provider.SetUserId(_userId);
        }

        foreach (var property in _userProperties)
        {
            provider.SetUserProperty(property.Key, property.Value);
        }
    }

    public void RemoveProvider(IAnalyticsProvider provider)
    {
        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        _providers.Remove(provider);
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

        foreach (var provider in _providers)
        {
            if (_userId != null)
            {
                provider.SetUserId(_userId);
            }

            foreach (var property in _userProperties)
            {
                provider.SetUserProperty(property.Key, property.Value);
            }
        }

        _isInitialized = true;
    }

    public void LogEvent(string name, IDictionary<string, object>? parameters = null)
    {
        foreach (var provider in _providers)
        {
            if (provider.IsInitialized)
            {
                provider.LogEvent(name, parameters);
            }
        }
    }

    public void LogEvent(AnalyticsEvent ev)
    {
        LogEvent(ev.Name, ev.Parameters);
    }

    public void SetUserProperty(string name, string value)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Property name cannot be null or empty.", nameof(name));
        }

        _userProperties[name] = value;

        foreach (var provider in _providers)
        {
            if (provider.IsInitialized)
            {
                provider.SetUserProperty(name, value);
            }
        }
    }

    public void SetUserId(string userId)
    {
        _userId = userId;

        foreach (var provider in _providers)
        {
            if (provider.IsInitialized)
            {
                provider.SetUserId(userId);
            }
        }
    }
}
