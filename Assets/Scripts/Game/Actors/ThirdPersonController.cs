using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors
{
    public class ThirdPersonController : ActorController
    {
#region Ground Check
        [Header("Ground Check")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _groundCheckStart;

        [SerializeField]
        [ReadOnly]
        private Vector3 _groundCheckEnd;

        [SerializeField]
        [ReadOnly]
        private float _groundCheckRadius;

        [SerializeField]
        private float _groundCheckEpsilon = 0.1f;

        [SerializeField]
        [ReadOnly]
        private LayerMask _groundCheckIgnoreLayerMask;

        [SerializeField]
        [ReadOnly]
        private bool _isGrounded;

        public bool IsGrounded => _isGrounded;
#endregion

        public ThirdPersonControllerData ControllerData { get; set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitRigidbody();

            _groundCheckIgnoreLayerMask = ~(1 << gameObject.layer);
        }

        private void FixedUpdate()
        {
            CheckGrounded();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.angularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.velocity);
        }
#endregion

        private void InitRigidbody()
        {
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            Rigidbody.detectCollisions = true;
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            Rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        private void CheckGrounded()
        {
            _groundCheckStart = Owner.Collider.bounds.center;
            float groundCheckY = Owner.Collider.bounds.min.y - _groundCheckEpsilon;

            _groundCheckEnd = new Vector3(_groundCheckStart.x, groundCheckY, _groundCheckStart.z);
            _groundCheckRadius = Owner.Collider.bounds.extents.x;
            _isGrounded = Physics.CheckCapsule(_groundCheckStart, _groundCheckEnd, _groundCheckRadius, _groundCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);
        }

        public override void RotateModel(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }
        }

        public override void Turn(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }
        }

        public override void Move(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }

            float speed = ControllerData.MoveSpeed;
            Rigidbody.MovePosition(transform.position + axes * speed * dt);
        }

        public void Jump()
        {
            if(!Owner.CanMove || !IsGrounded) {
                return;
            }
        }
    }
}
