namespace Nimbus.IAP;

/// <summary>
/// Defines a product for the IAP provider.
/// </summary>
public readonly struct IAPProductDefinition
{
    /// <summary>
    /// The unique identifier of the product (Store ID).
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// The type of the product.
    /// </summary>
    public IAPProductType Type { get; }

    /// <summary>
    /// The fallback price if the store is not available.
    /// </summary>
    public decimal FallbackPrice { get; }

    /// <summary>
    /// The fallback currency code (e.g., "USD").
    /// </summary>
    public string FallbackCurrencyCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IAPProductDefinition"/> struct.
    /// </summary>
    /// <param name="id">Product ID.</param>
    /// <param name="type">Product type.</param>
    /// <param name="fallbackPrice">Fallback price.</param>
    /// <param name="fallbackCurrencyCode">Fallback currency code.</param>
    public IAPProductDefinition(string id, IAPProductType type, decimal fallbackPrice = 0, string fallbackCurrencyCode = "USD")
    {
        Id = id;
        Type = type;
        FallbackPrice = fallbackPrice;
        FallbackCurrencyCode = fallbackCurrencyCode;
    }
}
