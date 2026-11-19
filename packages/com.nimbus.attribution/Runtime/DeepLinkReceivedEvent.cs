namespace Nimbus.Attribution;

public readonly struct DeepLinkReceivedEvent
{
    public string DeepLink { get; }

    public bool IsHandled { get; }

    public DeepLinkReceivedEvent(string deepLink, bool isHandled)
    {
        DeepLink = deepLink;
        IsHandled = isHandled;
    }
}
