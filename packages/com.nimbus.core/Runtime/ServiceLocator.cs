using System;
using System.Collections.Generic;

namespace Nimbus.Core;

public sealed class ServiceLocator
{
    private readonly Dictionary<Type, object> _services = new();

    public void Register<T>(T service)
    {
        var type = typeof(T);

        if (_services.ContainsKey(type))
        {
            throw new InvalidOperationException($"Service of type {type.Name} is already registered.");
        }

        _services[type] = service ?? throw new ArgumentNullException(nameof(service));
    }

    public void Unregister<T>()
    {
        var type = typeof(T);

        if (!_services.Remove(type))
        {
            throw new InvalidOperationException($"Service of type {type.Name} is not registered.");
        }
    }

    public T Get<T>()
    {
        var type = typeof(T);

        if (!_services.TryGetValue(type, out var service))
        {
            throw new KeyNotFoundException($"Service of type {type.Name} is not registered.");
        }

        return (T)service;
    }

    public bool TryGet<T>(out T service)
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out var result))
        {
            service = (T)result;
            return true;
        }

        service = default!;
        return false;
    }
}
