using Nimbus.Core;
using UnityEngine;

#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Nimbus.Haptics;

/// <summary>
/// Concrete implementation of IHapticsService.
/// Handles cross-platform haptic feedback using native APIs where available.
/// </summary>
public sealed class HapticsService : IHapticsService
{
    public bool IsEnabled { get; private set; } = true;

    /// <inheritdoc />
    public void SetEnabled(bool enabled)
    {
        IsEnabled = enabled;
        Log.Haptics.Info($"Haptics enabled: {enabled}");
    }

    /// <inheritdoc />
    public void Play(HapticPattern pattern)
    {
        if (!IsEnabled)
        {
            return;
        }

        Log.Haptics.Debug($"Playing haptic pattern: {pattern}");

#if UNITY_EDITOR
        // No haptics in editor
#elif UNITY_IOS
        PlayIosHaptic(pattern);
#elif UNITY_ANDROID
        PlayAndroidHaptic(pattern);
#else
        // Fallback for other platforms
        if (pattern == HapticPattern.Heavy || pattern == HapticPattern.Failure)
        {
            Handheld.Vibrate();
        }
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void Nimbus_PlayHaptic(int pattern);

    private void PlayIosHaptic(HapticPattern pattern)
    {
        // Map enum to int for native bridge
        Nimbus_PlayHaptic((int)pattern);
    }
#elif UNITY_IOS
    private void PlayIosHaptic(HapticPattern pattern) { }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass? _unityPlayer;
    private static AndroidJavaObject? _currentActivity;
    private static AndroidJavaObject? _vibrator;
    private static AndroidJavaClass? _vibrationEffect;
    private static int _apiLevel = -1;

    private void PlayAndroidHaptic(HapticPattern pattern)
    {
        try
        {
            InitializeAndroid();

            if (_vibrator == null) return;

            if (_apiLevel >= 26 && _vibrationEffect != null)
            {
                // API 26+ uses VibrationEffect
                long[]? timings = null;
                int[]? amplitudes = null;

                switch (pattern)
                {
                    case HapticPattern.Light: timings = new long[] { 0, 20 }; amplitudes = new int[] { 0, 40 }; break;
                    case HapticPattern.Medium: timings = new long[] { 0, 40 }; amplitudes = new int[] { 0, 80 }; break;
                    case HapticPattern.Heavy: timings = new long[] { 0, 60 }; amplitudes = new int[] { 0, 150 }; break;
                    case HapticPattern.Success: timings = new long[] { 0, 40, 40, 40 }; amplitudes = new int[] { 0, 80, 0, 150 }; break;
                    case HapticPattern.Warning: timings = new long[] { 0, 60, 40, 60 }; amplitudes = new int[] { 0, 150, 0, 100 }; break;
                    case HapticPattern.Failure: timings = new long[] { 0, 100, 40, 100 }; amplitudes = new int[] { 0, 200, 0, 200 }; break;
                    case HapticPattern.Selection: timings = new long[] { 0, 10 }; amplitudes = new int[] { 0, 30 }; break;
                }

                if (timings != null && amplitudes != null)
                {
                    using var effect = _vibrationEffect.CallStatic<AndroidJavaObject>("createWaveform", timings, amplitudes, -1);
                    _vibrator.Call("vibrate", effect);
                }
            }
            else
            {
                // Fallback for older APIs
                long duration = 40;
                switch (pattern)
                {
                    case HapticPattern.Light: duration = 20; break;
                    case HapticPattern.Medium: duration = 40; break;
                    case HapticPattern.Heavy: duration = 80; break;
                }
                _vibrator.Call("vibrate", duration);
            }
        }
        catch (System.Exception ex)
        {
            Log.Haptics.Error($"Android haptic error: {ex.Message}");
        }
    }

    private void InitializeAndroid()
    {
        if (_vibrator != null) return;

        _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        _vibrator = _currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        
        using var version = new AndroidJavaClass("android.os.Build$VERSION");
        _apiLevel = version.GetStatic<int>("SDK_INT");

        if (_apiLevel >= 26)
        {
            _vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
        }
    }
#elif UNITY_ANDROID
    private void PlayAndroidHaptic(HapticPattern pattern) { }
#endif
}
