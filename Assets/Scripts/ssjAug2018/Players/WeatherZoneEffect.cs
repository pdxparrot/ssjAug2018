using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class WeatherZoneEffect : MonoBehaviour
    {
        [SerializeField]
        private string _zoneType;

        [SerializeField]
        private ParticleSystem _vfx;

        [SerializeField]
        private AudioClip _audioClip;

#region Unity Lifecycle
        private void Start()
        {
            _vfx.Stop();
        }

        private void OnTriggerEnter(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            if(weather.WeatherZoneType == _zoneType) {
                _vfx.Play();
                AudioManager.Instance.PlayAmbient(_audioClip);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            if(weather.WeatherZoneType == _zoneType) {
                _vfx.Stop();
                AudioManager.Instance.StopAmbient();
            }
        }
#endregion
    }
}
