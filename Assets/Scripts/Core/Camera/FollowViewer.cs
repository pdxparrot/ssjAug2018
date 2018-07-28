using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(FollowCamera))]
    public abstract class FollowViewer : Viewer
    {
        private Rigidbody _rigidbody;

        private Collider _collider;

        private FollowCamera _followCamera;

        public FollowCamera FollowCamera => _followCamera;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody>();
            InitRigidbody();

            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            _followCamera = GetComponent<FollowCamera>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TODO: FollowViewer trigger!");
        }
#endregion

        private void InitRigidbody()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _rigidbody.detectCollisions = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            _rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        public void SetMinOrbitRadius(float orbitMinRadius)
        {
            _followCamera.OrbitMinRadius = orbitMinRadius;
        }

        public void SetMaxOrbitRadius(float orbitMaxRadius)
        {
            _followCamera.OrbitMaxRadius = orbitMaxRadius;
        }
    }
}
