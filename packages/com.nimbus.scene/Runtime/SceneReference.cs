using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Nimbus.Scene;

[Serializable]
public struct SceneReference
{
    [SerializeField]
    private AssetReferenceScene _sceneAsset;

    public AssetReferenceScene SceneAsset => _sceneAsset;

    public bool IsValid => _sceneAsset != null && _sceneAsset.RuntimeKeyIsValid();
}
