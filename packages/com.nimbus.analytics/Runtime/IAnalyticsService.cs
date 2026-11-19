using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.Analytics;

public interface IAnalyticsService : IService, IInitializable
{
    void AddProvider(IAnalyticsProvider provider);

    void RemoveProvider(IAnalyticsProvider provider);

    void LogEvent(string name, IDictionary<string, object>? parameters = null);

    void LogEvent(AnalyticsEvent ev);

    string? UserId { get; }

    IReadOnlyDictionary<string, string> UserProperties { get; }

    void SetUserProperty(string name, string value);

    void SetUserId(string userId);
}
