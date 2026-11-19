using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Nimbus.Core;

namespace Nimbus.Config;

/// <summary>
/// Utility for merging remote configuration JSON blobs into registered ScriptableObject configs.
/// Uses reflection to map fields marked with [RemoteOverridable] and provides a hook for manual merging.
/// </summary>
public static class ConfigMerger
{
    private static readonly Dictionary<Type, List<FieldMapping>> _cache = new();

    /// <summary>
    /// Applies a JSON object to a specific config found by key in the registry.
    /// </summary>
    /// <param name="key">The config key (matching GameConfigSO.Key).</param>
    /// <param name="json">The JSON object containing overrides.</param>
    public static void Apply(string key, JObject json)
    {
        if (json == null)
        {
            return;
        }

        if (Services.TryGet<IConfigRegistry>(out var registry))
        {
            if (registry.TryGetConfig<GameConfigSO>(key, out var config))
            {
                ApplyToConfig(config, json);
            }
            else
            {
                Log.Config.Warning($"Received remote overrides for unknown config key: {key}");
            }
        }
    }

    /// <summary>
    /// Applies a root JSON object containing multiple config overrides.
    /// The root object should have keys matching GameConfigSO keys, with their values being JObjects.
    /// </summary>
    /// <param name="root">The root JSON object.</param>
    public static void ApplyAll(JObject root)
    {
        if (root == null)
        {
            return;
        }

        if (Services.TryGet<IConfigRegistry>(out var registry))
        {
            foreach (var property in root.Properties())
            {
                if (registry.TryGetConfig<GameConfigSO>(property.Name, out var config))
                {
                    if (property.Value is JObject json)
                    {
                        ApplyToConfig(config, json);
                    }
                }
                else
                {
                    Log.Config.Warning($"Received remote overrides for unknown config key: {property.Name}");
                }
            }
        }
    }

    private static void ApplyToConfig(GameConfigSO config, JObject json)
    {
        Type type = config.GetType();
        if (!_cache.TryGetValue(type, out var mappings))
        {
            mappings = CreateMappings(type);
            _cache[type] = mappings;
        }

        foreach (var mapping in mappings)
        {
            if (json.TryGetValue(mapping.Key, out var token))
            {
                try
                {
                    object? value = token.ToObject(mapping.Field.FieldType);
                    mapping.Field.SetValue(config, value);
                }
                catch (Exception ex)
                {
                    Log.Config.Error($"Failed to apply remote override '{mapping.Key}' to {type.Name}: {ex.Message}");
                }
            }
        }

        config.ApplyRemoteOverrides(json);
    }

    private static List<FieldMapping> CreateMappings(Type type)
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
        public readonly string Key;
        public readonly FieldInfo Field;

        public FieldMapping(string key, FieldInfo field)
        {
            Key = key;
            Field = field;
        }
    }
}
