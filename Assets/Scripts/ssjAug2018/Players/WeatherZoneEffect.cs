using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class WeatherZoneEffect : MonoBehaviour
    {
        private readonly HashSet<WeatherZone> _zones = new HashSet<WeatherZone>();

        [SerializeField]
        [ReadOnly]
        private Transform _particleSystemParent;

        public Transform ParticleSystemParent { get { return _particleSystemParent; } set { _particleSystemParent = value; } }

#region Unity Lifecycle
        private void OnDestroy()
        {
            foreach(WeatherZone zone in _zones) {
                zone.Exit(gameObject);
            }
            _zones.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            weather.Enter(gameObject, ParticleSystemParent);
            _zones.Add(weather);
        }

        private void OnTriggerExit(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            weather.Exit(gameObject);
            _zones.Remove(weather);
        }
#endregion
    }
}
