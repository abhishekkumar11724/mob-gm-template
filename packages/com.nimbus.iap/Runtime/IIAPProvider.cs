using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.IAP;

/// <summary>
/// Abstract interface for a third-party in-app purchase provider.
/// Implementations live in separate packages (e.g., Unity IAP).
/// </summary>
public interface IIAPProvider
{
    /// <summary>
    /// Fired when a purchase is successfully processed by the store.
    /// This may be triggered by a direct purchase call or by the store's initialization/restoration.
    /// </summary>
    event Action<PurchaseResult> OnPurchaseProcessed;

    /// <summary>
    /// Fired when a purchase fails.
    /// </summary>
    event Action<string, string> OnPurchaseFailed;

    /// <summary>
    /// Gets a value indicating whether the provider is initialized and ready to process purchases.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initializes the IAP provider with the given product catalog.
    /// </summary>
    /// <param name="products">The list of products to initialize.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A result indicating success or failure of initialization.</returns>
    UniTask<Result<bool>> InitializeAsync(IReadOnlyList<IAPProductDefinition> products, CancellationToken ct);

    /// <summary>
    /// Requests the purchase of a product by its ID.
    /// </summary>
    /// <param name="productId">The store-specific product identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A result containing the purchase info or an error message.</returns>
    UniTask<Result<PurchaseResult>> PurchaseAsync(string productId, CancellationToken ct);

    /// <summary>
    /// Requests the restoration of previously purchased non-consumable products.
    /// Most relevant for iOS.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A result indicating whether the restoration request was successfully initiated.</returns>
    UniTask<Result<bool>> RestorePurchasesAsync(CancellationToken ct);
}
