using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Analytics;

/// <summary>
/// Abstract interface for a third-party analytics provider.
/// Implementations live in separate packages (e.g., Firebase, GameAnalytics).
/// </summary>
public interface IAnalyticsProvider
{
    /// <summary>
    /// Gets a value indicating whether the provider is initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initializes the analytics provider.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A UniTask representing the asynchronous operation.</returns>
    UniTask InitializeAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Logs a custom event with parameters.
    /// </summary>
    /// <param name="name">The name of the event.</param>
    /// <param name="parameters">Optional dictionary of parameters.</param>
    void LogEvent(string name, IDictionary<string, object>? parameters);

    /// <summary>
    /// Sets a user property value.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="value">The value of the property.</param>
    void SetUserProperty(string name, string value);

    /// <summary>
    /// Sets the user ID for this session.
    /// </summary>
    /// <param name="userId">The unique identifier for the user.</param>
    void SetUserId(string userId);
}
