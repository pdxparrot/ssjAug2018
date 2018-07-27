using System.Collections.Generic;
using pdxpartyparrot.ssjAug2018.World;
using UnityEngine;


namespace pdxparyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(LayerMask))]
    public class Platform : MonoBehaviour, IGrabbable {

        [SerializeField]
        private float _speed;

        [SerializeField]
        private List<Transform> _waypoints;

        private Transform _targetWayponit;
        private int _waypointIterator = 0;

        private Collider _collider;

        public Collider Collider => _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _targetWayponit = _waypoints[0];
            transform.LookAt(_targetWayponit);
        }

        private void FixedUpdate()
        {
            float step = _speed * Time.deltaTime;
            if(transform.position == _targetWayponit.position)
            {
                _waypointIterator = (_waypointIterator == _waypoints.Count) ? 0 : _waypointIterator + 1;
                _targetWayponit = _waypoints[_waypointIterator];
                this.transform.LookAt(_targetWayponit);
                Debug.Log("My target is now " + _targetWayponit.name);
            }
            transform.position = Vector3.MoveTowards(transform.position, _targetWayponit.position, step);
        }
    }
}