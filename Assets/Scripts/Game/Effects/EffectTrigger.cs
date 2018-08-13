using pdxpartyparrot.Core.Audio;

using UnityEngine;

namespace pdxpartyparrot.Game.Effects
{
    public sealed class EffectTrigger : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _vfx;

        [SerializeField]
        private AudioClip _audioClip;

        [SerializeField]
        private AudioSource _audioSource;

#region Unity Lifecycle
        private void Awake()
        {
            if(null != _audioSource) {
                AudioManager.Instance.InitSFXAudioMixerGroup(_audioSource);
            }
        }

        private void Start()
        {
            _vfx.Stop();
        }
#endregion

        public void Trigger()
        {
            _vfx.Play();

            if(null == _audioSource) {
                AudioManager.Instance.PlayOneShot(_audioClip);
            } else {
                _audioSource.clip = _audioClip;
                _audioSource.Play();
            }
        }
    }
}
