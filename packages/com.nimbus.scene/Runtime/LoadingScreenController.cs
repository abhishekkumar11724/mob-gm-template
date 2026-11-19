using Cysharp.Threading.Tasks;
using Nimbus.Core;
using Nimbus.UI;

namespace Nimbus.Scene;

/// <summary>
/// Controller for showing and hiding the loading screen during scene transitions.
/// </summary>
public sealed class LoadingScreenController
{
    // Note: The actual LoadingScreenView implementation is expected to be in the game project
    // or a future UI-kit package. This controller uses the IUIService abstraction.

    private readonly IUIService _uiService;

    public LoadingScreenController(IUIService uiService)
    {
        _uiService = uiService;
    }

    /// <summary>
    /// Shows the loading screen.
    /// </summary>
    /// <param name="style">Optional style name for the loading screen.</param>
    public async UniTask ShowAsync(string style = "default")
    {
        // Implementation might vary. It could be a specific ScreenView.
        // For now, we assume the game registers a view named 'LoadingScreen'.
        // Or we might need a more concrete way to identify the loading view.
        Log.Scene.Info($"Showing loading screen with style: {style}");
        
        // This is a placeholder for actual UI logic.
        // In a real implementation, we might call _uiService.ShowScreenAsync<LoadingScreenView>();
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// Hides the loading screen.
    /// </summary>
    public async UniTask HideAsync()
    {
        Log.Scene.Info("Hiding loading screen.");
        await UniTask.CompletedTask;
    }
}
