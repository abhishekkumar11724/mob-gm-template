using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.Scene;

public interface ISceneService : IService
{
    UniTask TransitionAsync(SceneLoadRequest request);

    void RegisterHook(ISceneTransitionHook hook);

    void UnregisterHook(ISceneTransitionHook hook);
}
