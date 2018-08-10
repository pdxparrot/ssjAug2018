using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class WeatherZone : MonoBehaviour
    {
        [SerializeField]
        private string _zoneType;

        public string WeatherZoneType => _zoneType;

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
