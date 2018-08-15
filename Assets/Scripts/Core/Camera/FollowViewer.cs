using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(FollowCamera))]
    public class FollowViewer : Viewer
    {
        private Rigidbody _rigidbody;

        private Collider _collider;

        public FollowCamera FollowCamera { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody>();
            InitRigidbody();

            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            FollowCamera = GetComponent<FollowCamera>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TODO: FollowViewer trigger!");
        }
#endregion

        private void InitRigidbody()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.detectCollisions = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            _rigidbody.interpolation = RigidbodyInterpolation.None;
        }
    }
}
