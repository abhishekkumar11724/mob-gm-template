using System.Collections.Generic;

namespace Nimbus.Attribution;

public readonly struct AttributionData
{
    public AttributionStatus Status { get; }

    public string Campaign { get; }

    public string MediaSource { get; }

    public string AdGroup { get; }

    public string AdSet { get; }

    public IReadOnlyDictionary<string, string> Properties { get; }

    public AttributionData(
        AttributionStatus status,
        string campaign,
        string mediaSource,
        string adGroup,
        string adSet,
        IReadOnlyDictionary<string, string> properties)
    {
        Status = status;
        Campaign = campaign ?? string.Empty;
        MediaSource = mediaSource ?? string.Empty;
        AdGroup = adGroup ?? string.Empty;
        AdSet = adSet ?? string.Empty;
        Properties = properties ?? new Dictionary<string, string>();
    }

    public bool IsOrganic => Status == AttributionStatus.Organic;
}
