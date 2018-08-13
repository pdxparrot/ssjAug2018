using JetBrains.Annotations;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class WeatherZone : MonoBehaviour
    {
        [SerializeField]
        [CanBeNull]
        private ParticleSystem _particleSystemPrefab;

        [CanBeNull]
        public ParticleSystem ParticleSystemPrefab => _particleSystemPrefab;

        [SerializeField]
        [CanBeNull]
        private AudioClip _audioClip;

        [CanBeNull]
        public AudioClip AudioClip => _audioClip;

        private BoxCollider _collider;

#region Unity Lifecycle
        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }

        private void OnDrawGizmos()
        {
            if(null != _collider) {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_collider.center, _collider.size);
            }
        }
#endregion
    }
}
