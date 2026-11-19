using System;
using UnityEngine.AddressableAssets;

namespace Nimbus.Scene;

/// <summary>
/// An <see cref="AssetReference"/> restricted to scene assets, so the Inspector
/// only accepts <c>.unity</c> files for a <see cref="SceneReference"/>.
/// </summary>
[Serializable]
public sealed class AssetReferenceScene : AssetReference
{
    public AssetReferenceScene(string guid)
        : base(guid)
    {
    }

    public override bool ValidateAsset(string path)
    {
        return path.EndsWith(".unity", StringComparison.OrdinalIgnoreCase);
    }
}
