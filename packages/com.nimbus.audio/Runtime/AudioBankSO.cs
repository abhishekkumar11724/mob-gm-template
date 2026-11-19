using System.Collections.Generic;
using UnityEngine;

namespace Nimbus.Audio;

[CreateAssetMenu(fileName = "AudioBank", menuName = "Nimbus/Audio/Audio Bank")]
public sealed class AudioBankSO : ScriptableObject
{
    [SerializeField] private List<MusicTrackSO> _musicTracks = new();
    [SerializeField] private List<SfxDefinitionSO> _sfxDefinitions = new();
    [SerializeField] private List<AudioClip> _clips = new();

    public IReadOnlyList<MusicTrackSO> MusicTracks => _musicTracks;

    public IReadOnlyList<SfxDefinitionSO> SfxDefinitions => _sfxDefinitions;

    public IReadOnlyList<AudioClip> Clips => _clips;
}
