namespace Nimbus.Ads;

/// <summary>
/// Event fired when an ad fails to load or show.
/// </summary>
public readonly struct AdFailedEvent
{
    /// <summary>
    /// The placement ID of the ad.
    /// </summary>
    public readonly string PlacementId;

    /// <summary>
    /// The type of the ad that failed.
    /// </summary>
    public readonly AdType AdType;

    /// <summary>
    /// The reason for the failure.
    /// </summary>
    public readonly string Reason;

    public AdFailedEvent(string placementId, AdType adType, string reason)
    {
        PlacementId = placementId;
        AdType = adType;
        Reason = reason;
    }
}
