using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Attribution;

public interface IAttributionProvider
{
    event Action<AttributionData> OnAttributionReceived;

    event Action<string> OnDeepLinkReceived;

    bool IsInitialized { get; }

    UniTask InitializeAsync(CancellationToken cancellationToken);

    void SetUserId(string userId);
}
