namespace Nimbus.Scene;

/// <summary>
/// Details for a scene transition request.
/// </summary>
public struct SceneLoadRequest
{
    /// <summary>
    /// The target scene to load.
    /// </summary>
    public SceneReference Target;

    /// <summary>
    /// Whether to show a loading screen during transition.
    /// </summary>
    public bool ShowLoadingScreen;

    /// <summary>
    /// Whether to attempt showing an interstitial ad during transition.
    /// </summary>
    public bool ShowInterstitialAd;

    /// <summary>
    /// Optional style/key for the loading screen UI.
    /// </summary>
    public string LoadingScreenStyle;

    /// <summary>
    /// Constructor for common cases.
    /// </summary>
    public SceneLoadRequest(SceneReference target, bool showLoadingScreen = true)
    {
        Target = target;
        ShowLoadingScreen = showLoadingScreen;
        ShowInterstitialAd = false;
        LoadingScreenStyle = "default";
    }
}
