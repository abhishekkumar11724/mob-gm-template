using System;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nimbus.Core;

/// <summary>
/// Central logging facade providing per-channel filtering and compile-time stripping for non-critical logs.
/// </summary>
public static class Log
{
    public enum LogLevel
    {
        Verbose = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        None = 5
    }

    /// <summary>
    /// Base class for log channels.
    /// </summary>
    public class Channel
    {
        private readonly string _name;
        private LogLevel _minimumLevel = LogLevel.Verbose;

        public Channel(string name)
        {
            _name = name;
        }

        public void SetMinimumLevel(LogLevel level)
        {
            _minimumLevel = level;
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        [Conditional("NIMBUS_ENABLE_VERBOSE_LOGS")]
        public void Verbose(string message, Object? context = null)
        {
            if (_minimumLevel <= LogLevel.Verbose)
            {
                UnityEngine.Debug.Log($"[V][{_name}] {message}", context);
            }
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        [Conditional("NIMBUS_ENABLE_DEBUG_LOGS")]
        public void Debug(string message, Object? context = null)
        {
            if (_minimumLevel <= LogLevel.Debug)
            {
                UnityEngine.Debug.Log($"[D][{_name}] {message}", context);
            }
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        [Conditional("NIMBUS_ENABLE_INFO_LOGS")]
        public void Info(string message, Object? context = null)
        {
            if (_minimumLevel <= LogLevel.Info)
            {
                UnityEngine.Debug.Log($"[I][{_name}] {message}", context);
            }
        }

        public void Warning(string message, Object? context = null)
        {
            if (_minimumLevel <= LogLevel.Warning)
            {
                UnityEngine.Debug.LogWarning($"[W][{_name}] {message}", context);
            }
        }

        public void Error(string message, Object? context = null)
        {
            if (_minimumLevel <= LogLevel.Error)
            {
                UnityEngine.Debug.LogError($"[E][{_name}] {message}", context);
            }
        }
    }

    public static readonly Channel Core = new Channel("Core");
    public static readonly Channel Config = new Channel("Config");
    public static readonly Channel Save = new Channel("Save");
    public static readonly Channel Audio = new Channel("Audio");
    public static readonly Channel Input = new Channel("Input");
    public static readonly Channel Scene = new Channel("Scene");
    public static readonly Channel Haptics = new Channel("Haptics");
    public static readonly Channel UI = new Channel("UI");
    public static readonly Channel Tween = new Channel("Tween");
    public static readonly Channel Localization = new Channel("Localization");
    public static readonly Channel Economy = new Channel("Economy");
    public static readonly Channel Progression = new Channel("Progression");
    public static readonly Channel Quests = new Channel("Quests");
    public static readonly Channel DailyRewards = new Channel("DailyRewards");
    public static readonly Channel Tutorial = new Channel("Tutorial");
    public static readonly Channel Shop = new Channel("Shop");
    public static readonly Channel Ads = new Channel("Ads");
    public static readonly Channel IAP = new Channel("IAP");
    public static readonly Channel Analytics = new Channel("Analytics");
    public static readonly Channel RemoteConfig = new Channel("RemoteConfig");
    public static readonly Channel Attribution = new Channel("Attribution");
    public static readonly Channel Consent = new Channel("Consent");
    public static readonly Channel Notifications = new Channel("Notifications");
    public static readonly Channel Social = new Channel("Social");
    public static readonly Channel DebugTools = new Channel("DebugTools");
}
