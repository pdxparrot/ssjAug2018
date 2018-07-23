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
        private Transform _groundCheckTransform;

        [SerializeField]
        private float _groundCheckRadius = 1;

        [SerializeField]
        [ReadOnly]
        private bool _isGrounded;

        public bool IsGrounded => _isGrounded;

        [SerializeField]
        [ReadOnly]
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
            UpdateIsGrounded();

            _isFalling = !IsGrounded && Rigidbody.velocity.y < 0.0f;

            // fudge our velocity a little so movememnt feels better
            FudgeVelocity();
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.angularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.velocity);

            Gizmos.color = IsGrounded ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GetGroundCheckCenter(), _groundCheckRadius);
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

#region Actions
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

            Rigidbody.velocity = transform.localRotation * new Vector3(axes.x * ControllerData.MoveSpeed, Rigidbody.velocity.y, axes.y * ControllerData.MoveSpeed);
        }

        public virtual void Jump(bool force=false)
        {
            if(!Owner.CanMove) {
                return;
            }

            if(!force && !IsGrounded) {
                return;
            }

            // TODO: so this is mathmatically correct, but it doesn't actually hit the height if we sqrt it...
            //Vector3 velocity = Vector3.up * (Mathf.Sqrt(ControllerData.JumpHeight * -2.0f * Physics.gravity.y));
            Vector3 velocity = Vector3.up * (ControllerData.JumpHeight * -2.0f * Physics.gravity.y);
            Rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        }
#endregion

#region Grounded Check
        protected Vector3 GetGroundCheckCenter()
        {
            Vector3 center = _groundCheckTransform != null ? _groundCheckTransform.position : transform.position;
            return new Vector3(center.x, center.y + _groundCheckRadius - 0.1f, center.z);
        }

        protected bool CheckIsGrounded(Vector3 center)
        {
            return Physics.CheckSphere(center, _groundCheckRadius, CollisionCheckIgnoreLayerMask, QueryTriggerInteraction.Ignore);;
        }

        private void UpdateIsGrounded()
        {
            _isGrounded = CheckIsGrounded(GetGroundCheckCenter());
        }
#endregion

        private void FudgeVelocity()
        {
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
    }
}
