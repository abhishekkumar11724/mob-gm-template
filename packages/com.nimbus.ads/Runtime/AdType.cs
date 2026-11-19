namespace Nimbus.Ads;

/// <summary>
/// Defines the different types of ads supported by the system.
/// </summary>
public enum AdType
{
    /// <summary>
    /// A full-screen interstitial ad.
    /// </summary>
    Interstitial,

    /// <summary>
    /// A rewarded video ad that grants a reward upon completion.
    /// </summary>
    Rewarded,

    /// <summary>
    /// A small banner ad usually placed at the top or bottom.
    /// </summary>
    Banner,

    /// <summary>
    /// A medium rectangle ad (300x250).
    /// </summary>
    Mrec
}
