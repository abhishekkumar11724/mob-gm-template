namespace Nimbus.IAP;

/// <summary>
/// Contains the result of a purchase operation.
/// </summary>
public readonly struct PurchaseResult
{
    /// <summary>
    /// The ID of the purchased product.
    /// </summary>
    public string ProductId { get; }

    /// <summary>
    /// The receipt blob from the store (JSON or base64).
    /// </summary>
    public string Receipt { get; }

    /// <summary>
    /// The transaction ID from the store.
    /// </summary>
    public string TransactionId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PurchaseResult"/> struct.
    /// </summary>
    /// <param name="productId">Product ID.</param>
    /// <param name="receipt">Store receipt.</param>
    /// <param name="transactionId">Store transaction ID.</param>
    public PurchaseResult(string productId, string receipt, string transactionId)
    {
        ProductId = productId;
        Receipt = receipt;
        TransactionId = transactionId;
    }
}
