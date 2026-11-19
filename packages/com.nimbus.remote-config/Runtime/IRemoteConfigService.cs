using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.RemoteConfig;

public interface IRemoteConfigService : IService, IInitializable
{
    bool HasProvider { get; }

    void SetProvider(IRemoteConfigProvider provider);

    UniTask<Result<bool>> FetchAsync(CancellationToken cancellationToken);

    string? GetValue(string key);

    T GetValue<T>(string key, T defaultValue);

    string? GetJson();

    void SetDefault(string key, string value);

    void SetDefaults(IDictionary<string, string> defaults);
}
