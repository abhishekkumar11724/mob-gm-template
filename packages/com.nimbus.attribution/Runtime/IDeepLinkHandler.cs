namespace Nimbus.Attribution;

public interface IDeepLinkHandler
{
    bool CanHandle(string deepLink);

    void Handle(string deepLink);
}
