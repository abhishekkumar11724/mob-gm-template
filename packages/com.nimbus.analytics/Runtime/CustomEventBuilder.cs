using System;
using System.Collections.Generic;
using Nimbus.Core;

namespace Nimbus.Analytics;

public static class Analytics
{
    public static CustomEventBuilder Event(string name)
    {
        return CustomEventBuilder.Create(name);
    }
}

public struct CustomEventBuilder
{
    private static readonly Pool<Dictionary<string, object>> _dictionaryPool = new(
        () => new Dictionary<string, object>(8),
        null,
        d => d.Clear()
    );

    private readonly string _name;
    private Dictionary<string, object>? _parameters;

    private CustomEventBuilder(string name)
    {
        _name = name;
        _parameters = null;
    }

    internal static CustomEventBuilder Create(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Event name cannot be null or empty.", nameof(name));
        }

        return new CustomEventBuilder(name);
    }

    public CustomEventBuilder Add(string key, object value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Parameter key cannot be null or empty.", nameof(key));
        }

        _parameters ??= _dictionaryPool.Get();
        _parameters[key] = value;
        return this;
    }

    public void Send()
    {
        var service = Services.Get<IAnalyticsService>();
        service.LogEvent(_name, _parameters);

        if (_parameters != null)
        {
            _dictionaryPool.Return(_parameters);
            _parameters = null;
        }
    }
}
