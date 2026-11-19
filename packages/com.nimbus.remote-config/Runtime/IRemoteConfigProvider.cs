using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.RemoteConfig;

public interface IRemoteConfigProvider
{
    bool IsInitialized { get; }

    UniTask InitializeAsync(CancellationToken cancellationToken);

    UniTask<Result<bool>> FetchAsync(CancellationToken cancellationToken);

    string? GetValue(string key);

    string? GetJson();
}
