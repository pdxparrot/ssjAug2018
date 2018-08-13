using System.Collections.Generic;

using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class WeatherZoneEffect : MonoBehaviour
    {
        private readonly HashSet<WeatherZone> _zones = new HashSet<WeatherZone>();

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

            weather.Enter(gameObject);
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
