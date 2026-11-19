namespace Nimbus.Ads;

/// <summary>
/// Event fired when the user clicks on an ad.
/// </summary>
public readonly struct AdClickedEvent
{
    /// <summary>
    /// The placement ID of the ad.
    /// </summary>
    public readonly string PlacementId;

    /// <summary>
    /// The type of the ad that was clicked.
    /// </summary>
    public readonly AdType AdType;

    public AdClickedEvent(string placementId, AdType adType)
    {
        PlacementId = placementId;
        AdType = adType;
    }
}
