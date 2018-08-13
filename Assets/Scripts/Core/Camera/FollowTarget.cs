using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
// TODO: make 2D vs 3D subclasses (probably need to abstract this)
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Collider _collider;

        public Collider Collider => _collider;

        [SerializeField]
        private Transform _targetTransform;

        public Transform TargetTransform { get { return null == _targetTransform ? transform : _targetTransform; } set { _targetTransform = value; } }

        [SerializeField]
        [ReadOnly]
        private  Vector3 _lastLookAxes;

        public Vector3 LastLookAxes
        {
            get { return _lastLookAxes; }

            set { _lastLookAxes = value; }
        }
    }
}
