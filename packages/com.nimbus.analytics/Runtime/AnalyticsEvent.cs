using System.Collections.Generic;

namespace Nimbus.Analytics;

public readonly struct AnalyticsEvent
{
    public readonly string Name;
    public readonly IDictionary<string, object>? Parameters;

    public AnalyticsEvent(string name, IDictionary<string, object>? parameters)
    {
        Name = name;
        Parameters = parameters;
    }
}
