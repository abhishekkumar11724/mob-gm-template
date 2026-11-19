using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nimbus.Config;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Nimbus.Config.Editor;

public sealed class ConfigBrowserWindow : EditorWindow
{
    private List<GameConfigSO> _configs = new();
    private Vector2 _scrollPosition;
    private string _simulatedJson = "{}";
    private GameConfigSO? _selectedConfig;
    private bool _showSimulate;

    [MenuItem("Nimbus/Config Browser")]
    public static void ShowWindow()
    {
        GetWindow<ConfigBrowserWindow>("Config Browser");
    }

    private void OnEnable()
    {
        RefreshConfigs();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Width(250));
        DrawConfigList();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        DrawDetailPanel();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void RefreshConfigs()
    {
        _configs.Clear();
        string[] guids = AssetDatabase.FindAssets("t:GameConfigSO");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var config = AssetDatabase.LoadAssetAtPath<GameConfigSO>(path);
            if (config != null)
            {
                _configs.Add(config);
            }
        }

        _configs = _configs.OrderBy(c => c.Key).ToList();
    }

    private void DrawConfigList()
    {
        DrawListToolbar();
        DrawValidationSummary();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        var duplicateKeys = GetDuplicateKeys();
        foreach (var config in _configs)
        {
            DrawConfigButton(config, duplicateKeys);
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawListToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        EditorGUILayout.LabelField("Configs", EditorStyles.boldLabel);
        if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
        {
            RefreshConfigs();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawValidationSummary()
    {
        var duplicateKeys = GetDuplicateKeys();
        if (duplicateKeys.Count > 0)
        {
            EditorGUILayout.HelpBox($"Duplicate keys found: {string.Join(", ", duplicateKeys)}", MessageType.Error);
        }
    }

    private List<string> GetDuplicateKeys()
    {
        return _configs.GroupBy(c => c.Key)
            .Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key))
            .Select(g => g.Key)
            .ToList();
    }

    private void DrawConfigButton(GameConfigSO config, List<string> duplicateKeys)
    {
        bool isDuplicate = duplicateKeys.Contains(config.Key);
        bool isMissingKey = string.IsNullOrEmpty(config.Key);

        EditorGUILayout.BeginHorizontal();
        GUIStyle style = new GUIStyle(EditorStyles.label);
        if (config == _selectedConfig)
        {
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.cyan;
        }

        string label = $"{config.Key} ({config.name})";
        if (isMissingKey)
        {
            label = $"[MISSING KEY] ({config.name})";
        }

        if (GUILayout.Button(label, style))
        {
            _selectedConfig = config;
            GUI.FocusControl(null);
        }

        if (isDuplicate || isMissingKey)
        {
            EditorGUILayout.LabelField("⚠️", GUILayout.Width(20));
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawDetailPanel()
    {
        if (_selectedConfig == null)
        {
            EditorGUILayout.LabelField("Select a config to see details.", EditorStyles.centeredGreyMiniLabel);
            return;
        }

        DrawConfigHeader(_selectedConfig);
        EditorGUILayout.Space();
        DrawOverridableFields(_selectedConfig);
        EditorGUILayout.Space();
        DrawSimulation(_selectedConfig);
    }

    private void DrawConfigHeader(GameConfigSO config)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField($"Config: {config.name}", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Key: {config.Key}");
        EditorGUILayout.LabelField($"Version: {config.Version}");
        EditorGUILayout.EndVertical();
    }

    private void DrawOverridableFields(GameConfigSO config)
    {
        EditorGUILayout.LabelField("Overridable Fields", EditorStyles.boldLabel);
        var fields = GetOverridableFields(config.GetType());
        if (fields.Count == 0)
        {
            EditorGUILayout.LabelField("No fields marked with [RemoteOverridable].", EditorStyles.miniLabel);
            return;
        }

        foreach (var field in fields)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(field.RemoteKey, GUILayout.Width(150));
            EditorGUILayout.LabelField($"-> {field.Field.Name} ({field.Field.FieldType.Name})", EditorStyles.miniLabel);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawSimulation(GameConfigSO config)
    {
        _showSimulate = EditorGUILayout.BeginFoldoutHeaderGroup(_showSimulate, "Simulate Remote Overrides");
        if (_showSimulate)
        {
            EditorGUILayout.LabelField("JSON Overlay", EditorStyles.miniLabel);
            _simulatedJson = EditorGUILayout.TextArea(_simulatedJson, GUILayout.Height(100));

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply to Selected"))
            {
                SimulateApply(config);
            }

            if (GUILayout.Button("Apply to All (Root JSON)"))
            {
                SimulateApplyAll();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.HelpBox("Simulation applies changes in memory. Use AssetDatabase.SaveAssets() or save the SO to persist if desired.", MessageType.Info);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void SimulateApply(GameConfigSO config)
    {
        try
        {
            JObject json = JObject.Parse(_simulatedJson);
            InvokeApplyToConfig(config, json);
            EditorUtility.SetDirty(config);
            Debug.Log($"[ConfigBrowser] Applied simulated JSON to {config.Key}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ConfigBrowser] Failed to apply simulation: {ex.Message}");
        }
    }

    private void SimulateApplyAll()
    {
        try
        {
            JObject root = JObject.Parse(_simulatedJson);
            foreach (var property in root.Properties())
            {
                var config = _configs.FirstOrDefault(c => c.Key == property.Name);
                if (config != null && property.Value is JObject json)
                {
                    InvokeApplyToConfig(config, json);
                    EditorUtility.SetDirty(config);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ConfigBrowser] Failed to apply simulation: {ex.Message}");
        }
    }

    private void InvokeApplyToConfig(GameConfigSO config, JObject json)
    {
        var method = typeof(ConfigMerger).GetMethod("ApplyToConfig", BindingFlags.Static | BindingFlags.NonPublic);
        method?.Invoke(null, new object[] { config, json });
    }

    private List<FieldMapping> GetOverridableFields(Type type)
    {
        var list = new List<FieldMapping>();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            var attr = field.GetCustomAttribute<RemoteOverridableAttribute>();
            if (attr != null)
            {
                string key = attr.CustomKey ?? field.Name.TrimStart('_');
                list.Add(new FieldMapping(key, field));
            }
        }

        return list;
    }

    private readonly struct FieldMapping
    {
        public readonly string RemoteKey;
        public readonly FieldInfo Field;

        public FieldMapping(string key, FieldInfo field)
        {
            RemoteKey = key;
            Field = field;
        }
    }
}
