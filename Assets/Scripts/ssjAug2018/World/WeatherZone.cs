using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class WeatherZone : MonoBehaviour
    {
        public enum ZoneType
        {
            None,
            Snow
        }

        [SerializeField]
        private ZoneType _zoneType = ZoneType.None;

        public ZoneType WeatherZoneType => _zoneType;

#region Unity Lifecycle
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }
#endregion
    }
}
