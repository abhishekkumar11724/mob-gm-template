using System;
using UnityEngine;

namespace Nimbus.Localization;

[Serializable]
public struct LocalizationEntry
{
    [SerializeField]
    private string _key;

    [SerializeField]
    [TextArea(3, 10)]
    private string _value;

    public LocalizationEntry(string key, string value)
    {
        _key = key;
        _value = value;
    }

    public string Key => _key ?? string.Empty;

    public string Value => _value ?? string.Empty;
}
