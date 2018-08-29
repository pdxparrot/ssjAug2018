using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
// TODO: rename this to WorldZone or something and move it to Game
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(BoxCollider))]
    public sealed class WeatherZone : MonoBehaviour
    {
        [SerializeField]
        [CanBeNull]
        private ParticleSystem _particleSystemPrefab;

        [SerializeField]
        [CanBeNull]
        private AudioClip _audioClip;

        private AudioSource _audioSource;

        private BoxCollider _collider;

        private readonly Dictionary<GameObject, ParticleSystem> _zoneParticleSystems = new Dictionary<GameObject, ParticleSystem>();

#region Unity Lifecycle
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            AudioManager.Instance.InitAmbientAudioMixerGroup(_audioSource);
            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
            _audioSource.spatialBlend = 0.0f;
            _audioSource.clip = _audioClip;

            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }

        private void OnDrawGizmos()
        {
            if(null != _collider) {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_collider.bounds.center, _collider.bounds.size);
            }
        }
#endregion

        public void Enter(GameObject obj, Transform parent=null)
        {
            if(_zoneParticleSystems.ContainsKey(obj)) {
                Debug.LogWarning($"Duplicate zone {name} particle systems for {obj.name}");
            }

            if(null != _particleSystemPrefab) {
                ParticleSystem zoneParticleSystem = Instantiate(_particleSystemPrefab, null == parent ? obj.transform : parent);
                _zoneParticleSystems.Add(obj, zoneParticleSystem);
            }

            _audioSource.Play();
        }

        public void Exit(GameObject obj)
        {
            ParticleSystem zoneParticleSystem;
            if(_zoneParticleSystems.Remove(obj, out zoneParticleSystem) && null != zoneParticleSystem) {
                Destroy(zoneParticleSystem.gameObject);
            }

            // null check just in case we're shutting down
            if(null != _audioSource) {
                _audioSource.Stop();
            }
        }
    }
}
