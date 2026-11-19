namespace Nimbus.Ads;

/// <summary>
/// Event fired when an ad is successfully shown to the user.
/// </summary>
public readonly struct AdShownEvent
{
    /// <summary>
    /// The placement ID of the ad.
    /// </summary>
    public readonly string PlacementId;

    /// <summary>
    /// The type of the ad that was shown.
    /// </summary>
    public readonly AdType AdType;

    public AdShownEvent(string placementId, AdType adType)
    {
        PlacementId = placementId;
        AdType = adType;
    }
}
