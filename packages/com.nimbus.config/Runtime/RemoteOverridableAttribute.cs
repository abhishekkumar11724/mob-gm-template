using System;

namespace Nimbus.Config;

[AttributeUsage(AttributeTargets.Field)]
public sealed class RemoteOverridableAttribute : Attribute
{
    public string? CustomKey { get; }

    public RemoteOverridableAttribute(string? customKey = null)
    {
        CustomKey = customKey;
    }
}
