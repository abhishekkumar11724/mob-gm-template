using System.Diagnostics;
using Nimbus.Core;

namespace Nimbus.DebugTools;

public static class DebugTools
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD || NIMBUS_ENABLE_DEBUG_TOOLS
    public const bool IsEnabled = true;
#else
    public const bool IsEnabled = false;
#endif

    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    [Conditional("NIMBUS_ENABLE_DEBUG_TOOLS")]
    public static void Initialize()
    {
        Services.Register<IDebugService>(new DebugService());
    }

    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    [Conditional("NIMBUS_ENABLE_DEBUG_TOOLS")]
    public static void Register(IDebugEntry entry)
    {
        if (Services.TryGet<IDebugService>(out var service))
        {
            service.Register(entry);
        }
    }

    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    [Conditional("NIMBUS_ENABLE_DEBUG_TOOLS")]
    public static void Unregister(IDebugEntry entry)
    {
        if (Services.TryGet<IDebugService>(out var service))
        {
            service.Unregister(entry);
        }
    }

    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    [Conditional("NIMBUS_ENABLE_DEBUG_TOOLS")]
    public static void SetVisible(bool visible)
    {
        if (Services.TryGet<IDebugService>(out var service))
        {
            service.SetVisible(visible);
        }
    }
}
