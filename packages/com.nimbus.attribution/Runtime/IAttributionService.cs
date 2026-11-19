using System;
using Nimbus.Core;

namespace Nimbus.Attribution;

public interface IAttributionService : IService
{
    AttributionData? AttributionData { get; }

    void SetProvider(IAttributionProvider provider);

    void RegisterHandler(IDeepLinkHandler handler);

    void UnregisterHandler(IDeepLinkHandler handler);

    event Action<string>? OnDeepLinkReceived;

    void SetUserId(string userId);
}
