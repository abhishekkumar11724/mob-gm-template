using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nimbus.Audio;

/// <summary>
/// Concrete implementation of the IAudioService.
/// Manages music crossfading, SFX pooling, and volume controls.
/// </summary>
public sealed class AudioService : IAudioService, IInitializable, IDisposable
{
    private const int InitialSfxPoolSize = 10;

    private readonly GameObjectPool _sfxPool;
    private readonly AudioSource _musicSourceA;
    private readonly AudioSource _musicSourceB;

    private bool _isSourceAActive = true;
    private float _masterVolume = 1.0f;
    private float _musicVolume = 1.0f;
    private float _sfxVolume = 1.0f;
    private bool _isMuted = false;
    private float _duckFactor = 1.0f;

    private CancellationTokenSource? _musicFadeCts;
    private CancellationTokenSource? _duckCts;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioService"/> class.
    /// </summary>
    /// <param name="poolParent">Optional parent transform for pooled objects.</param>
    public AudioService(Transform? poolParent = null)
    {
        // Setup SFX Pool
        var sfxGo = new GameObject("SfxSourceTemplate");
        var templateSource = sfxGo.AddComponent<AudioSource>();
        templateSource.playOnAwake = false;

        if (poolParent != null)
        {
            sfxGo.transform.SetParent(poolParent);
        }
        sfxGo.SetActive(false);

        _sfxPool = new GameObjectPool(sfxGo, poolParent, null, source =>
        {
            var audioSource = source.GetComponent<AudioSource>();
            audioSource.Stop();
            audioSource.clip = null;
        }, InitialSfxPoolSize);

        Object.Destroy(sfxGo);

        // Setup Music Sources
        var musicRoot = new GameObject("MusicSources");
        if (poolParent != null)
        {
            musicRoot.transform.SetParent(poolParent);
        }
        Object.DontDestroyOnLoad(musicRoot);

        _musicSourceA = musicRoot.AddComponent<AudioSource>();
        _musicSourceB = musicRoot.AddComponent<AudioSource>();

        _musicSourceA.loop = true;
        _musicSourceB.loop = true;
        _musicSourceA.playOnAwake = false;
        _musicSourceB.playOnAwake = false;
        _musicSourceA.volume = 0;
        _musicSourceB.volume = 0;
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public AudioService() : this(null)
    {
    }

    /// <inheritdoc />
    public UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        Log.Audio.Info("AudioService initialized.");
        return UniTask.CompletedTask;
    }

    /// <inheritdoc />
    public void PlayMusic(MusicTrackSO musicTrack, float fadeSeconds = 1.0f)
    {
        if (musicTrack == null)
        {
            throw new ArgumentNullException(nameof(musicTrack));
        }

        _musicFadeCts?.Cancel();
        _musicFadeCts = new CancellationTokenSource();

        var nextSource = _isSourceAActive ? _musicSourceB : _musicSourceA;
        var currentSource = _isSourceAActive ? _musicSourceA : _musicSourceB;

        nextSource.clip = musicTrack.Clip;
        nextSource.loop = musicTrack.Loop;
        nextSource.Play();

        _isSourceAActive = !_isSourceAActive;

        FadeMusicAsync(currentSource, nextSource, musicTrack.Volume, fadeSeconds, _musicFadeCts.Token).Forget();
    }

    /// <inheritdoc />
    public void StopMusic(float fadeSeconds = 1.0f)
    {
        _musicFadeCts?.Cancel();
        _musicFadeCts = new CancellationTokenSource();

        var currentSource = _isSourceAActive ? _musicSourceA : _musicSourceB;
        FadeOutAsync(currentSource, fadeSeconds, _musicFadeCts.Token).Forget();
    }

    /// <inheritdoc />
    public void PlaySfx(SfxDefinitionSO sfx)
    {
        if (sfx == null || sfx.Clips.Count == 0)
        {
            return;
        }

        var clip = sfx.Clips[UnityEngine.Random.Range(0, sfx.Clips.Count)];
        var go = _sfxPool.Get();
        var source = go.GetComponent<AudioSource>();

        source.clip = clip;
        source.volume = sfx.Volume * GetCombinedSfxVolume();
        source.pitch = sfx.Pitch + UnityEngine.Random.Range(-sfx.PitchRandomization, sfx.PitchRandomization);
        source.Play();

        // Duration calculation might need adjustment if pitch is very high/low
        float effectiveDuration = clip.length / Mathf.Max(0.01f, Mathf.Abs(source.pitch));
        WaitAndReturnSfxAsync(go, effectiveDuration).Forget();
    }

    /// <inheritdoc />
    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        UpdateMusicVolumes();
    }

    /// <inheritdoc />
    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        UpdateMusicVolumes();
    }

    /// <inheritdoc />
    public void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        // Note: Active SFX sources are not tracked individually for volume updates
        // only new SFX calls will reflect the change immediately.
    }

    /// <inheritdoc />
    public void SetMuted(bool muted)
    {
        _isMuted = muted;
        UpdateMusicVolumes();
    }

    /// <inheritdoc />
    public void Duck(float targetVolume, float durationSeconds = 2.0f)
    {
        _duckCts?.Cancel();
        _duckCts = new CancellationTokenSource();
        DuckAsync(targetVolume, durationSeconds, _duckCts.Token).Forget();
    }

    private async UniTaskVoid FadeMusicAsync(AudioSource current, AudioSource next, float targetVolume, float duration, CancellationToken ct)
    {
        float elapsed = 0;
        float startNextVolume = next.volume;
        float startCurrentVolume = current.volume;

        while (elapsed < duration)
        {
            if (ct.IsCancellationRequested)
                return;

            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;

            next.volume = Mathf.Lerp(startNextVolume, targetVolume * GetCombinedMusicVolume(), t);
            current.volume = Mathf.Lerp(startCurrentVolume, 0, t);

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        next.volume = targetVolume * GetCombinedMusicVolume();
        current.volume = 0;
        current.Stop();
        current.clip = null;
    }

    private async UniTaskVoid FadeOutAsync(AudioSource source, float duration, CancellationToken ct)
    {
        float elapsed = 0;
        float startVolume = source.volume;

        while (elapsed < duration)
        {
            if (ct.IsCancellationRequested)
                return;

            elapsed += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, elapsed / duration);

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        source.volume = 0;
        source.Stop();
        source.clip = null;
    }

    private async UniTaskVoid WaitAndReturnSfxAsync(GameObject go, float duration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.UnscaledDeltaTime);
        if (go != null)
        {
            _sfxPool.Return(go);
        }
    }

    private async UniTaskVoid DuckAsync(float targetFactor, float duration, CancellationToken ct)
    {
        float elapsed = 0;
        float startFactor = _duckFactor;

        while (elapsed < duration)
        {
            if (ct.IsCancellationRequested)
                return;

            elapsed += Time.unscaledDeltaTime;
            _duckFactor = Mathf.Lerp(startFactor, targetFactor, elapsed / duration);
            UpdateMusicVolumes();

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        _duckFactor = targetFactor;
        UpdateMusicVolumes();
    }

    private void UpdateMusicVolumes()
    {
        float musicVol = GetCombinedMusicVolume();
        if (_musicSourceA.isPlaying)
            _musicSourceA.volume = musicVol;
        if (_musicSourceB.isPlaying)
            _musicSourceB.volume = musicVol;
    }

    private float GetCombinedMusicVolume() => _isMuted ? 0 : _masterVolume * _musicVolume * _duckFactor;
    private float GetCombinedSfxVolume() => _isMuted ? 0 : _masterVolume * _sfxVolume;

    /// <inheritdoc />
    public void Dispose()
    {
        _musicFadeCts?.Cancel();
        _musicFadeCts?.Dispose();
        _duckCts?.Cancel();
        _duckCts?.Dispose();
        _sfxPool.Dispose();
    }
}
