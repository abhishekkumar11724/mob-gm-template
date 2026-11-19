using Nimbus.Config;
using Newtonsoft.Json.Linq;

namespace Nimbus.Progression;

public abstract class XPCurveSO : GameConfigSO
{
    public override string Key => "xp_curve";

    public override int Version => 1;

    public override void ApplyRemoteOverrides(JObject json)
    {
    }

    public abstract int GetXPRequiredForLevel(int level);
}
