using Nimbus.Core;

namespace Nimbus.Audio;

public interface IAudioService : IService
{
    void PlayMusic(MusicTrackSO musicTrack, float fadeSeconds = 1.0f);

    void StopMusic(float fadeSeconds = 1.0f);

    void PlaySfx(SfxDefinitionSO sfx);

    void SetMasterVolume(float volume);

    void SetMusicVolume(float volume);

    void SetSfxVolume(float volume);

    void SetMuted(bool muted);

    void Duck(float targetVolume, float durationSeconds = 2.0f);
}
