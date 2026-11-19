using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Nimbus.Core;

public sealed class AsyncEvent<T>
{
    private readonly List<Func<T, UniTask>> _handlers = new();

    public void Subscribe(Func<T, UniTask> handler)
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

    public void Unsubscribe(Func<T, UniTask> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        _handlers.Remove(handler);
    }

    public async UniTask InvokeAsync(T eventData)
    {
        for (var i = 0; i < _handlers.Count; i++)
        {
            await _handlers[i].Invoke(eventData);
        }
    }

    public async UniTask InvokeParallelAsync(T eventData)
    {
        var tasks = new UniTask[_handlers.Count];
        for (var i = 0; i < _handlers.Count; i++)
        {
            tasks[i] = _handlers[i].Invoke(eventData);
        }

        await UniTask.WhenAll(tasks);
    }
}
