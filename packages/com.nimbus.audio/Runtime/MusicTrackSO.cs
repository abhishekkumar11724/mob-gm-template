using UnityEngine;

namespace Nimbus.Audio;

[CreateAssetMenu(fileName = "MusicTrack", menuName = "Nimbus/Audio/Music Track")]
public sealed class MusicTrackSO : ScriptableObject
{
    [SerializeField] private AudioClip _clip = null!;
    [SerializeField][Range(0.0f, 1.0f)] private float _volume = 1.0f;
    [SerializeField] private bool _loop = true;

    public AudioClip Clip => _clip;

    public float Volume => _volume;

    public bool Loop => _loop;
}
