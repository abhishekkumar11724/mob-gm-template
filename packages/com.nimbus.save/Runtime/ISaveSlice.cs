using Newtonsoft.Json.Linq;

namespace Nimbus.Save;

public interface ISaveSlice
{
    string Key { get; }

    int Version { get; }

    JObject Serialize();

    void Deserialize(JObject data);

    void Migrate(int fromVersion, JObject data);
}
