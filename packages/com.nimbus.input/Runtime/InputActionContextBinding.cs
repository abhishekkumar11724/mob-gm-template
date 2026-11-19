using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nimbus.Input;

[Serializable]
public sealed class InputActionContextBinding
{
    [SerializeField] private string _contextName = string.Empty;
    [SerializeField] private InputActionAsset _asset = null!;
    [SerializeField] private string[] _actionMapNames = Array.Empty<string>();

    public string ContextName => _contextName;

    public InputActionAsset Asset => _asset;

    public string[] ActionMapNames => _actionMapNames;
}
