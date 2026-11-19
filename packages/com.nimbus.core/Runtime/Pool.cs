using System;
using System.Collections.Generic;

namespace Nimbus.Core;

public sealed class Pool<T> : IPool<T>
{
    private readonly Stack<T> _items = new();
    private readonly Func<T> _factory;
    private readonly Action<T>? _onGet;
    private readonly Action<T>? _onReturn;

    public Pool(Func<T> factory, Action<T>? onGet = null, Action<T>? onReturn = null, int initialCapacity = 0)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _onGet = onGet;
        _onReturn = onReturn;

        for (var i = 0; i < initialCapacity; i++)
        {
            _items.Push(_factory());
        }
    }

    public T Get()
    {
        var item = _items.Count > 0 ? _items.Pop() : _factory();
        _onGet?.Invoke(item);
        return item;
    }

    public void Return(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _onReturn?.Invoke(item);
        _items.Push(item);
    }
}
