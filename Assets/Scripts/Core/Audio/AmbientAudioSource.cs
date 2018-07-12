using UnityEngine;

namespace pdxpartyparrot.Core.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbientAudioSource : MonoBehaviour
    {
        private AudioSource[] _audioSources;

#region Unity Lifecycle
        private void Awake()
        {
            _audioSources = GetComponents<AudioSource>();
            foreach(AudioSource source in _audioSources) {
                AudioManager.Instance.InitSFXAudioMixerGroup(source);
            }
        }
#endregion
    }
}
