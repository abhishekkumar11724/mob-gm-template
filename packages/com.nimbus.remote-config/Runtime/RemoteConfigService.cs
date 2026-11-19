using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.RemoteConfig;

public sealed class RemoteConfigService : IRemoteConfigService
{
    private IRemoteConfigProvider? _provider;
    private readonly Dictionary<string, string> _defaults = new();
    private bool _isInitialized;

    public bool HasProvider => _provider != null;

    public void SetProvider(IRemoteConfigProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public async UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        if (_isInitialized)
        {
            return;
        }

        if (_provider != null && !_provider.IsInitialized)
        {
            await _provider.InitializeAsync(cancellationToken);
        }

        _isInitialized = true;
    }

    public async UniTask<Result<bool>> FetchAsync(CancellationToken cancellationToken)
    {
        if (_provider == null)
        {
            return Result<bool>.Failure("Remote config provider is not set.");
        }

        if (!_provider.IsInitialized)
        {
            await _provider.InitializeAsync(cancellationToken);
        }

        return await _provider.FetchAsync(cancellationToken);
    }

    public string? GetValue(string key)
    {
        var value = _provider?.GetValue(key);
        if (value == null && _defaults.TryGetValue(key, out var defaultValue))
        {
            return defaultValue;
        }

        return value;
    }

    public T GetValue<T>(string key, T defaultValue)
    {
        var stringValue = GetValue(key);
        if (stringValue == null)
        {
            return defaultValue;
        }

        try
        {
            var type = typeof(T);
            if (type == typeof(bool))
            {
                if (bool.TryParse(stringValue, out var boolValue))
                {
                    return (T)(object)boolValue;
                }

                return defaultValue;
            }

            return (T)Convert.ChangeType(stringValue, type, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public string? GetJson()
    {
        return _provider?.GetJson();
    }

    public void SetDefault(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }

        _defaults[key] = value;
    }

    public void SetDefaults(IDictionary<string, string> defaults)
    {
        if (defaults == null)
        {
            throw new ArgumentNullException(nameof(defaults));
        }

        foreach (var kvp in defaults)
        {
            SetDefault(kvp.Key, kvp.Value);
        }
    }
}
