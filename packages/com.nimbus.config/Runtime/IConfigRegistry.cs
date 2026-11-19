using System.Collections.Generic;
using Nimbus.Core;

namespace Nimbus.Config;

public interface IConfigRegistry : IService
{
    T GetConfig<T>(string key) where T : GameConfigSO;

    bool TryGetConfig<T>(string key, out T config) where T : GameConfigSO;

    IEnumerable<GameConfigSO> GetAllConfigs();
}
