using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors
{
// TODO: this has nothing to do with ThirdPerson or whatever, just merge it all into ActorController and kill this class
    public class ThirdPersonController : ActorController
    {
        [SerializeField]
        [ReadOnly]
        private LayerMask _collisionCheckIgnoreLayerMask;

        protected LayerMask CollisionCheckIgnoreLayerMask => _collisionCheckIgnoreLayerMask;

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
        [ReadOnly]
        private bool _isGrounded;

        public bool IsGrounded => _isGrounded;

        [SerializeField]
        private bool _isFalling;

        public bool IsFalling => _isFalling;
#endregion

        public ThirdPersonControllerData ControllerData { get; set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitRigidbody();

            // prevent self-collision when doing cast checks
            _collisionCheckIgnoreLayerMask = ~(1 << gameObject.layer);
        }

        protected virtual void FixedUpdate()
        {
            CheckGrounded();

            _isFalling = !IsGrounded && Rigidbody.velocity.y < 0.0f;

            Vector3 adjustedVelocity = Rigidbody.velocity;

            // do some fudging to jumping/falling so it feels better
            if(!IsGrounded) {
                adjustedVelocity.y -= ControllerData.FallSpeedAdjustment;
            }

            // apply terminal velocity
            if(adjustedVelocity.y < -ControllerData.TerminalVelocity) {
                adjustedVelocity.y = -ControllerData.TerminalVelocity;
            }

            Rigidbody.velocity = adjustedVelocity;
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.angularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.velocity);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_groundCheckStart, _groundCheckRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_groundCheckEnd, _groundCheckRadius);
        }
#endregion

        public void Initialize(IActor actor, ThirdPersonControllerData data)
        {
            base.Initialize(actor);

            ControllerData = data;
        }

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
            Vector3 center = Owner.Collider.bounds.center;
            Vector3 min = Owner.Collider.bounds.min;
            Vector3 extents = Owner.Collider.bounds.extents;

            _groundCheckRadius = extents.x - 0.1f;
            _groundCheckStart = new Vector3(center.x, min.y + _groundCheckRadius + 0.1f, center.z);
            _groundCheckEnd   = new Vector3(center.x, min.y + _groundCheckRadius - ControllerData.GroundedCheckEpsilon, center.z);

            _isGrounded = Physics.CheckCapsule(_groundCheckStart, _groundCheckEnd, _groundCheckRadius, CollisionCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);
        }

        public override void Turn(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }

// TODO
        }

        public override void Move(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }

            if(!ControllerData.AllowAirControl && !IsGrounded) {
                return;
            }

            Vector3 velocity = axes * ControllerData.MoveSpeed;
            velocity.y = Rigidbody.velocity.y;

            Rigidbody.velocity = velocity;
        }

        public virtual void Jump()
        {
            if(!Owner.CanMove) {
                return;
            }

            if(!IsGrounded) {
                return;
            }

            // TODO: so this is mathmatically correct, but it doesn't actually hit the height if we sqrt it...
            //Vector3 velocity = Vector3.up * Mathf.Sqrt(ControllerData.JumpHeight * -2.0f * Physics.gravity.y);
            Vector3 velocity = Vector3.up * ControllerData.JumpHeight * -2.0f * Physics.gravity.y;
            Rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        }
    }
}
