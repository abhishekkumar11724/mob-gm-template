using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nimbus.Localization;

[CreateAssetMenu(fileName = "LocalizationTable", menuName = "Nimbus/Localization/Table")]
public sealed class LocalizationTableSO : ScriptableObject
{
    [SerializeField]
    private string _languageCode = "en";

    [SerializeField]
    private string _languageName = "English";

    [SerializeField]
    private List<LocalizationEntry> _entries = new();

    public string LanguageCode => _languageCode;

    public string LanguageName => _languageName;

    public IReadOnlyList<LocalizationEntry> Entries => _entries;

    public string? GetValue(LocKey key)
    {
        if (key.IsEmpty)
        {
            return null;
        }

        foreach (var entry in _entries)
        {
            if (entry.Key == key.Key)
            {
                return entry.Value;
            }
        }

        return null;
    }
}
