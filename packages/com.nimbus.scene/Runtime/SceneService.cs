using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Nimbus.Core;
using Nimbus.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Nimbus.Scene;

/// <summary>
/// Concrete implementation of ISceneService.
/// Orchestrates scene transitions with loading screens and lifecycle hooks.
/// </summary>
public sealed class SceneService : ISceneService
{
    private readonly List<ISceneTransitionHook> _hooks = new();
    private readonly LoadingScreenController _loadingController;
    private SceneInstance _currentSceneInstance;

    /// <summary>
    /// Initializes a new instance of the <see cref="SceneService"/> class.
    /// </summary>
    /// <param name="uiService">The UI service for managing loading screens.</param>
    public SceneService(IUIService uiService)
    {
        _loadingController = new LoadingScreenController(uiService);
    }

    /// <inheritdoc />
    public async UniTask TransitionAsync(SceneLoadRequest request)
    {
        if (!request.Target.IsValid)
        {
            throw new ArgumentException("SceneLoadRequest Target is invalid.");
        }

        Log.Scene.Info($"Transitioning to scene: {request.Target.SceneAsset.RuntimeKey}");

        // 1. Show Loading Screen
        if (request.ShowLoadingScreen)
        {
            await _loadingController.ShowAsync(request.LoadingScreenStyle);
        }

        // 2. Before Unload Hooks
        foreach (var hook in _hooks)
        {
            await hook.OnBeforeUnloadAsync();
        }

        // 3. Unload current active scene
        if (_currentSceneInstance.Scene.IsValid())
        {
            Log.Scene.Debug($"Unloading current scene: {_currentSceneInstance.Scene.name}");
            await Addressables.UnloadSceneAsync(_currentSceneInstance);
        }

        // 4. Before Load Hooks
        foreach (var hook in _hooks)
        {
            await hook.OnBeforeLoadAsync();
        }

        // 5. Load new scene additively
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(request.Target.SceneAsset, LoadSceneMode.Additive);
        _currentSceneInstance = await handle;

        if (!_currentSceneInstance.Scene.IsValid())
        {
            throw new Exception($"Failed to load scene: {request.Target.SceneAsset.RuntimeKey}");
        }

        // 6. Set as active scene
        SceneManager.SetActiveScene(_currentSceneInstance.Scene);
        Log.Scene.Debug($"New active scene: {_currentSceneInstance.Scene.name}");

        // 7. After Load Hooks
        foreach (var hook in _hooks)
        {
            await hook.OnAfterLoadAsync();
        }

        // 8. Hide Loading Screen
        if (request.ShowLoadingScreen)
        {
            await _loadingController.HideAsync();
        }
        
        Log.Scene.Info("Scene transition completed.");
    }

    /// <inheritdoc />
    public void RegisterHook(ISceneTransitionHook hook)
    {
        if (hook == null)
        {
            throw new ArgumentNullException(nameof(hook));
        }

        if (!_hooks.Contains(hook))
        {
            _hooks.Add(hook);
        }
    }

    /// <inheritdoc />
    public void UnregisterHook(ISceneTransitionHook hook)
    {
        if (hook == null)
        {
            throw new ArgumentNullException(nameof(hook));
        }

        _hooks.Remove(hook);
    }
}
