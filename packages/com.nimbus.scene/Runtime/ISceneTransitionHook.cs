using Cysharp.Threading.Tasks;

namespace Nimbus.Scene;

/// <summary>
/// Hook for participating in the scene transition lifecycle.
/// </summary>
public interface ISceneTransitionHook
{
    /// <summary>
    /// Called before the current active scene is unloaded.
    /// </summary>
    UniTask OnBeforeUnloadAsync();

    /// <summary>
    /// Called after the old scene is unloaded but before the new scene starts loading.
    /// </summary>
    UniTask OnBeforeLoadAsync();

    /// <summary>
    /// Called after the new scene is successfully loaded and set as active.
    /// </summary>
    UniTask OnAfterLoadAsync();
}
