using System.Collections.Generic;
using UnityEngine;

namespace Nimbus.Audio;

[CreateAssetMenu(fileName = "SfxDefinition", menuName = "Nimbus/Audio/SFX Definition")]
public sealed class SfxDefinitionSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> _clips = new();
    [SerializeField][Range(0.0f, 1.0f)] private float _volume = 1.0f;
    [SerializeField][Range(0.1f, 3.0f)] private float _pitch = 1.0f;
    [SerializeField][Range(0.0f, 0.5f)] private float _pitchRandomization = 0.0f;

    public IReadOnlyList<AudioClip> Clips => _clips;

    public float Volume => _volume;

    public float Pitch => _pitch;

    public float PitchRandomization => _pitchRandomization;
}
