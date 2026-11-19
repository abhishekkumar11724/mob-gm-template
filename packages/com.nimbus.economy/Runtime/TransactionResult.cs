using Nimbus.Core;

namespace Nimbus.Economy;

/// <summary>
/// Represents the result of an economy transaction.
/// </summary>
public readonly struct TransactionResult
{
    private readonly Result<bool> _result;

    private TransactionResult(Result<bool> result)
    {
        _result = result;
    }

    /// <summary>
    /// Gets a value indicating whether the transaction was successful.
    /// </summary>
    public bool IsSuccess => _result.IsSuccess;

    /// <summary>
    /// Gets a value indicating whether the transaction failed.
    /// </summary>
    public bool IsFailure => _result.IsFailure;

    /// <summary>
    /// Gets the error message if the transaction failed.
    /// </summary>
    public string Error => _result.Error;

    /// <summary>
    /// Creates a successful transaction result.
    /// </summary>
    /// <returns>A successful transaction result.</returns>
    public static TransactionResult Success()
    {
        return new TransactionResult(Result<bool>.Success(true));
    }

    /// <summary>
    /// Creates a failed transaction result with the specified error message.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <returns>A failed transaction result.</returns>
    public static TransactionResult Failure(string error)
    {
        return new TransactionResult(Result<bool>.Failure(error));
    }

    /// <summary>
    /// Creates a failed transaction result indicating insufficient funds.
    /// </summary>
    /// <returns>A failed transaction result.</returns>
    public static TransactionResult InsufficientFunds()
    {
        return Failure("Insufficient funds");
    }

    /// <summary>
    /// Creates a failed transaction result indicating an invalid amount.
    /// </summary>
    /// <returns>A failed transaction result.</returns>
    public static TransactionResult InvalidAmount()
    {
        return Failure("Invalid amount");
    }
}
