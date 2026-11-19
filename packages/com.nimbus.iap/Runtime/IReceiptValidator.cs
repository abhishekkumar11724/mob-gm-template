using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.IAP;

/// <summary>
/// Abstraction for validating in-app purchase receipts, usually via a backend server.
/// </summary>
public interface IReceiptValidator
{
    /// <summary>
    /// Validates a store receipt.
    /// </summary>
    /// <param name="purchase">The purchase result containing the receipt data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A result indicating whether the receipt is valid.</returns>
    UniTask<Result<bool>> ValidateAsync(PurchaseResult purchase, CancellationToken ct);
}
