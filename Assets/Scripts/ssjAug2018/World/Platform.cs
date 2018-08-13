using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace pdxparyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Platform : NetworkBehaviour, IGrabbable
    {
        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        private PlatformWaypoint[] _waypoints;

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _currentWaypointIndex;

        public Collider Collider { get; private set; }

#region Unity Lifecycle
        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if(_currentWaypointIndex < 0 || _currentWaypointIndex >= _waypoints.Length) {
                _currentWaypointIndex = 0;
                return;
            }

            PlatformWaypoint targetWaypoint = _waypoints[_currentWaypointIndex];
            if((targetWaypoint.transform.position - transform.position).sqrMagnitude < float.Epsilon) {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                targetWaypoint = _waypoints[_currentWaypointIndex];
            }

            transform.LookAt(targetWaypoint.transform);

            float step = _speed * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.transform.position, step);
        }
#endregion
    }
}
