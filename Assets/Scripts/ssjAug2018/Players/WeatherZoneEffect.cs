using JetBrains.Annotations;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class WeatherZoneEffect : MonoBehaviour
    {
        [CanBeNull]
        private ParticleSystem _particleSystem;

#region Unity Lifecycle
        private void OnTriggerEnter(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            if(null != weather.ParticleSystemPrefab) {
                _particleSystem = Instantiate(weather.ParticleSystemPrefab);
                if(null != _particleSystem) {
                    _particleSystem.Play();
                }
            }

            AudioManager.Instance.PlayAmbient(weather.AudioClip);
        }

        private void OnTriggerExit(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            AudioManager.Instance.StopAmbient();

            if(null != _particleSystem) {
                Destroy(_particleSystem.gameObject);
            }
            _particleSystem = null;
        }
#endregion
    }
}
