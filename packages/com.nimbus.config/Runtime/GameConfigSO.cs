using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Nimbus.Config;

public abstract class GameConfigSO : ScriptableObject
{
    public abstract string Key { get; }

    public abstract int Version { get; }

    public abstract void ApplyRemoteOverrides(JObject json);
}
