namespace Nimbus.IAP;

/// <summary>
/// Defines the types of in-app products.
/// </summary>
public enum IAPProductType
{
    /// <summary>
    /// A product that can be purchased multiple times (e.g., currency).
    /// </summary>
    Consumable,

    /// <summary>
    /// A product that is purchased once and remains forever (e.g., remove ads).
    /// </summary>
    NonConsumable,

    /// <summary>
    /// A product that provides benefits for a limited time (e.g., battle pass).
    /// </summary>
    Subscription
}
