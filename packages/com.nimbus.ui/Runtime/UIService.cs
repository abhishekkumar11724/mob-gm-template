using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Nimbus.UI;

/// <summary>
/// Concrete implementation of IUIService.
/// Manages a screen stack and a priority-based popup queue.
/// </summary>
public sealed class UIService : IUIService, IInitializable
{
    private readonly Dictionary<Type, ViewBase> _viewCache = new();
    private readonly Stack<ScreenView> _screenStack = new();
    private readonly List<PopupRequestInfo> _popupQueue = new();
    private PopupView? _activePopup;
    private Transform? _root;

    private struct PopupRequestInfo
    {
        public Type Type;
        public PopupRequest Request;
        public UniTaskCompletionSource<PopupView> CompletionSource;
    }

    /// <inheritdoc />
    public UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        Log.UI.Info("UIService initialized.");
        return UniTask.CompletedTask;
    }

    /// <inheritdoc />
    public void SetRoot(Transform root)
    {
        _root = root;
    }

    /// <inheritdoc />
    public async UniTask<T> ShowScreenAsync<T>() where T : ScreenView
    {
        var screen = await GetOrLoadViewAsync<T>();

        if (_screenStack.Count > 0)
        {
            var current = _screenStack.Peek();
            if (current == screen)
            {
                return screen;
            }
            await current.HideAsync();
        }

        _screenStack.Push(screen);
        await screen.ShowAsync();
        
        Log.UI.Info($"Showing screen: {typeof(T).Name}");
        return screen;
    }

    /// <inheritdoc />
    public async UniTask HideScreenAsync<T>() where T : ScreenView
    {
        if (_screenStack.Count == 0)
        {
            return;
        }

        var top = _screenStack.Peek();
        if (top is not T)
        {
            Log.UI.Warning($"Cannot hide screen {typeof(T).Name}: it is not at the top of the stack.");
            return;
        }

        _screenStack.Pop();
        await top.HideAsync();

        if (_screenStack.Count > 0)
        {
            var previous = _screenStack.Peek();
            await previous.ShowAsync();
        }
        
        Log.UI.Info($"Hidden screen: {typeof(T).Name}");
    }

    /// <inheritdoc />
    public async UniTask<T> ShowPopupAsync<T>(PopupRequest request = default) where T : PopupView
    {
        var tcs = new UniTaskCompletionSource<PopupView>();
        _popupQueue.Add(new PopupRequestInfo
        {
            Type = typeof(T),
            Request = request,
            CompletionSource = tcs
        });

        // Sort by priority descending
        _popupQueue.Sort((a, b) => b.Request.Priority.CompareTo(a.Request.Priority));

        ProcessPopupQueue().Forget();

        var result = await tcs.Task;
        return (T)result;
    }

    /// <inheritdoc />
    public async UniTask ClosePopupAsync(PopupView popup)
    {
        if (_activePopup == popup)
        {
            Log.UI.Info($"Closing active popup: {popup.GetType().Name}");
            await popup.HideAsync();
            _activePopup = null;
            ProcessPopupQueue().Forget();
        }
    }

    /// <inheritdoc />
    public async UniTask CloseAllPopupsAsync()
    {
        Log.UI.Info("Closing all popups.");
        _popupQueue.Clear();

        if (_activePopup != null)
        {
            await _activePopup.HideAsync();
            _activePopup = null;
        }
    }

    private async UniTaskVoid ProcessPopupQueue()
    {
        if (_activePopup != null || _popupQueue.Count == 0)
        {
            return;
        }

        var nextInfo = _popupQueue[0];
        _popupQueue.RemoveAt(0);

        var popup = await GetOrLoadViewAsync(nextInfo.Type);
        _activePopup = (PopupView)popup;
        
        Log.UI.Info($"Processing popup from queue: {nextInfo.Type.Name}");
        await _activePopup.ShowAsync();
        nextInfo.CompletionSource.TrySetResult(_activePopup);
    }

    private async UniTask<T> GetOrLoadViewAsync<T>() where T : ViewBase
    {
        var result = await GetOrLoadViewAsync(typeof(T));
        return (T)result;
    }

    private async UniTask<ViewBase> GetOrLoadViewAsync(Type type)
    {
        if (_viewCache.TryGetValue(type, out var view))
        {
            return view;
        }

        // Conventional addressable key: the type name
        var address = type.Name;
        Log.UI.Info($"Loading view prefab from Addressables: {address}");
        
        var handle = Addressables.LoadAssetAsync<GameObject>(address);
        var prefab = await handle.ToUniTask();
        
        var instance = Object.Instantiate(prefab, _root).GetComponent<ViewBase>();
        if (instance == null)
        {
            throw new InvalidOperationException($"Prefab for {type.Name} does not have a component inheriting from ViewBase.");
        }

        instance.gameObject.SetActive(false);
        _viewCache[type] = instance;
        return instance;
    }
}
