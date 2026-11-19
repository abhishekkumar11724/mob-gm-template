namespace Nimbus.Attribution;

public readonly struct InstallAttributed
{
    public AttributionData Data { get; }

    public InstallAttributed(AttributionData data)
    {
        Data = data;
    }
}
