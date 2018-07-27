using System.Collections.Generic;
using pdxpartyparrot.ssjAug2018.World;
using UnityEngine;


namespace pdxparyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(Collider))]
    public class Platform : MonoBehaviour, IGrabbable {

        [SerializeField]
        private float _speed = 5;

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
            float step = _speed * Time.fixedDeltaTime;
            if((transform.position -_targetWayponit.position).sqrMagnitude < Mathf.Pow(float.Epsilon, 2))
            {
                _waypointIterator = (_waypointIterator + 1) % _waypoints.Count;
                _targetWayponit = _waypoints[_waypointIterator];
                this.transform.LookAt(_targetWayponit);
            }
            transform.position = Vector3.MoveTowards(transform.position, _targetWayponit.position, step);
        }
    }
}