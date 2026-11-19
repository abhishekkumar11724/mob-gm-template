namespace Nimbus.Ads;

/// <summary>
/// Event fired when a rewarded ad has been completed and the reward should be granted.
/// </summary>
public readonly struct RewardedCompletedEvent
{
    /// <summary>
    /// The placement ID of the rewarded ad.
    /// </summary>
    public readonly string PlacementId;

    public RewardedCompletedEvent(string placementId)
    {
        PlacementId = placementId;
    }
}
