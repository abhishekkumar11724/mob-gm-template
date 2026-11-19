using System.Collections.Generic;

namespace Nimbus.Analytics;

public static class StandardEvents
{
    public static AnalyticsEvent LevelStart(int level)
    {
        return new AnalyticsEvent("level_start", new Dictionary<string, object>
        {
            ["level"] = level
        });
    }

    public static AnalyticsEvent LevelComplete(int level, int stars, float time)
    {
        return new AnalyticsEvent("level_complete", new Dictionary<string, object>
        {
            ["level"] = level,
            ["stars"] = stars,
            ["time"] = time
        });
    }

    public static AnalyticsEvent LevelFail(int level)
    {
        return new AnalyticsEvent("level_fail", new Dictionary<string, object>
        {
            ["level"] = level
        });
    }

    public static AnalyticsEvent AdShown(string placement, string type)
    {
        return new AnalyticsEvent("ad_shown", new Dictionary<string, object>
        {
            ["placement"] = placement,
            ["type"] = type
        });
    }

    public static AnalyticsEvent Purchase(string sku, decimal price, string currency)
    {
        return new AnalyticsEvent("purchase", new Dictionary<string, object>
        {
            ["sku"] = sku,
            ["price"] = (double)price,
            ["currency"] = currency
        });
    }

    public static AnalyticsEvent TutorialStep(string id)
    {
        return new AnalyticsEvent("tutorial_step", new Dictionary<string, object>
        {
            ["id"] = id
        });
    }
}
