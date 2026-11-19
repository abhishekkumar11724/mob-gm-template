using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Nimbus.UI;

public abstract class ViewBase : MonoBehaviour
{
    public event Action? OnShowing;
    public event Action? OnShown;
    public event Action? OnHiding;
    public event Action? OnHidden;

    public bool IsVisible { get; private set; }

    public virtual async UniTask ShowAsync(CancellationToken cancellationToken = default)
    {
        if (IsVisible)
        {
            return;
        }

        OnShowing?.Invoke();
        IsVisible = true;
        gameObject.SetActive(true);

        await OnShowAsync(cancellationToken);

        OnShown?.Invoke();
    }

    public virtual async UniTask HideAsync(CancellationToken cancellationToken = default)
    {
        if (!IsVisible)
        {
            return;
        }

        OnHiding?.Invoke();
        IsVisible = false;

        await OnHideAsync(cancellationToken);

        gameObject.SetActive(false);
        OnHidden?.Invoke();
    }

    protected virtual UniTask OnShowAsync(CancellationToken cancellationToken) => UniTask.CompletedTask;

    protected virtual UniTask OnHideAsync(CancellationToken cancellationToken) => UniTask.CompletedTask;
}
