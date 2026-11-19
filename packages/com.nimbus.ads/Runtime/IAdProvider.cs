using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Ads;

/// <summary>
/// Abstract interface for a third-party ad network provider.
/// Implementations live in separate packages (e.g., AppLovin, AdMob).
/// </summary>
public interface IAdProvider
{
    /// <summary>
    /// Fired when an ad has been successfully shown.
    /// </summary>
    event Action<string, AdType> OnAdShown;

    /// <summary>
    /// Fired when an ad failed to load or show.
    /// </summary>
    event Action<string, AdType, string> OnAdFailed;

    /// <summary>
    /// Fired when an ad was clicked.
    /// </summary>
    event Action<string, AdType> OnAdClicked;

    /// <summary>
    /// Fired when a rewarded ad has been completed and the reward should be granted.
    /// </summary>
    event Action<string> OnRewardedCompleted;

    /// <summary>
    /// Gets a value indicating whether the provider is initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initializes the ad provider.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A UniTask representing the asynchronous operation.</returns>
    UniTask InitializeAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Checks if an interstitial ad is loaded and ready to show.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    /// <returns>True if the ad is ready, otherwise false.</returns>
    bool IsInterstitialReady(string placementId);

    /// <summary>
    /// Requests the provider to load an interstitial ad.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    void LoadInterstitial(string placementId);

    /// <summary>
    /// Requests the provider to show a loaded interstitial ad.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    void ShowInterstitial(string placementId);

    /// <summary>
    /// Checks if a rewarded ad is loaded and ready to show.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    /// <returns>True if the ad is ready, otherwise false.</returns>
    bool IsRewardedReady(string placementId);

    /// <summary>
    /// Requests the provider to load a rewarded ad.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    void LoadRewarded(string placementId);

    /// <summary>
    /// Requests the provider to show a loaded rewarded ad.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    void ShowRewarded(string placementId);

    /// <summary>
    /// Requests the provider to show a banner ad.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    /// <param name="position">The position of the banner.</param>
    void ShowBanner(string placementId, BannerPosition position);

    /// <summary>
    /// Requests the provider to hide the banner ad.
    /// </summary>
    void HideBanner();

    /// <summary>
    /// Requests the provider to show an MREC ad.
    /// </summary>
    /// <param name="placementId">The placement ID for the ad.</param>
    /// <param name="position">The position of the MREC.</param>
    void ShowMrec(string placementId, BannerPosition position);

    /// <summary>
    /// Requests the provider to hide the MREC ad.
    /// </summary>
    void HideMrec();
}
