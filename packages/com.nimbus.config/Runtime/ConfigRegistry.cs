using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;
using UnityEngine.AddressableAssets;

namespace Nimbus.Config;

public sealed class ConfigRegistry : IConfigRegistry, IInitializable
{
    private readonly Dictionary<string, GameConfigSO> _configs = new();

    public async UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        var locations = await Addressables.LoadResourceLocationsAsync("config").WithCancellation(cancellationToken);

        if (locations.Count == 0)
        {
            return;
        }

        var tasks = new List<UniTask<GameConfigSO>>();
        foreach (var location in locations)
        {
            tasks.Add(Addressables.LoadAssetAsync<GameConfigSO>(location).ToUniTask());
        }

        var loadedConfigs = await UniTask.WhenAll(tasks);

        foreach (var config in loadedConfigs)
        {
            if (_configs.ContainsKey(config.Key))
            {
                Log.Config.Error($"Duplicate config key found: {config.Key}. Skipping {config.name}.");
                continue;
            }

            _configs.Add(config.Key, config);
        }
    }

    public T GetConfig<T>(string key) where T : GameConfigSO
    {
        if (_configs.TryGetValue(key, out var config))
        {
            return (T)config;
        }

        throw new KeyNotFoundException($"Config with key '{key}' not found in registry.");
    }

    public bool TryGetConfig<T>(string key, out T config) where T : GameConfigSO
    {
        if (_configs.TryGetValue(key, out var foundConfig))
        {
            config = (T)foundConfig;
            return true;
        }

        config = default!;
        return false;
    }

    public IEnumerable<GameConfigSO> GetAllConfigs()
    {
        return _configs.Values;
    }
}
