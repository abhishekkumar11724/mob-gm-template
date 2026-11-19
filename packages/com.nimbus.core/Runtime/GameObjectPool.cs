using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nimbus.Core;

public sealed class GameObjectPool : IPool<GameObject>, IDisposable
{
    private readonly Stack<GameObject> _items = new();
    private readonly GameObject _prefab;
    private readonly Transform? _parent;
    private readonly Action<GameObject>? _onGet;
    private readonly Action<GameObject>? _onReturn;

    public GameObjectPool(GameObject prefab, Transform? parent = null, Action<GameObject>? onGet = null, Action<GameObject>? onReturn = null, int initialCapacity = 0)
    {
        _prefab = prefab ? prefab : throw new ArgumentNullException(nameof(prefab));
        _parent = parent;
        _onGet = onGet;
        _onReturn = onReturn;

        for (var i = 0; i < initialCapacity; i++)
        {
            _items.Push(CreateNew());
        }
    }

    public GameObject Get()
    {
        var item = _items.Count > 0 ? _items.Pop() : CreateNew();
        item.SetActive(true);
        _onGet?.Invoke(item);
        return item;
    }

    public void Return(GameObject item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _onReturn?.Invoke(item);
        item.SetActive(false);
        
        if (_parent != null)
        {
            item.transform.SetParent(_parent);
        }

        _items.Push(item);
    }

    public void Dispose()
    {
        while (_items.Count > 0)
        {
            var item = _items.Pop();
            if (item != null)
            {
                Object.Destroy(item);
            }
        }
    }

    private GameObject CreateNew()
    {
        var item = Object.Instantiate(_prefab, _parent);
        item.SetActive(false);
        return item;
    }
}
