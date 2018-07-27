﻿using System.Collections;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors
{
// TODO: a lot of this is commong with FPS controllers, so probably a good place to do a subclass
    public class ThirdPersonController : ActorController
    {
        [SerializeField]
        private ThirdPersonControllerData _controllerData;

        protected ThirdPersonControllerData ControllerData => _controllerData;

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("How often to run raycast checks, in seconds")]
        private float _raycastRoutineRate = 0.1f;

        protected float RaycastRoutineRate => _raycastRoutineRate;

        [Space(10)]

#region Ground Check
        [Header("Ground Check")]

        [SerializeField]
        private Transform _groundCheckTransform;

        [SerializeField]
        private float _groundCheckRadius = 1.0f;

        [SerializeField]
        [ReadOnly]
        private bool _isGrounded;

        public bool IsGrounded => _isGrounded;

        [SerializeField]
        [ReadOnly]
        private bool _isFalling;

        public bool IsFalling => _isFalling;

        protected Vector3 GroundCheckCenter
        {
            get
            {
                Vector3 center = _groundCheckTransform.position;
                return new Vector3(center.x, center.y + _groundCheckRadius - ControllerData.GroundedCheckEpsilon, center.z);
            }
        }
#endregion

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private bool _isRunning;

        public bool IsRunning => _isRunning;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitRigidbody();
        }

        protected virtual void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            _isFalling = !IsGrounded && Rigidbody.velocity.y < 0.0f;

            FudgeVelocity(dt);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Rigidbody.position, transform.position + Rigidbody.angularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Rigidbody.position, transform.position + Rigidbody.velocity);

            Gizmos.color = IsGrounded ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter, _groundCheckRadius);
        }
#endregion

        public override void Initialize(IActor actor)
        {
            base.Initialize(actor);

            StartCoroutine(RaycastRoutine());
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
        public override void AnimationMove(Vector3 axes, float dt)
        {
            Vector3 forward = new Vector3(axes.x, 0.0f, axes.y);

            // align the movement with the camera
            if(null != Owner.Viewer) {
                forward = (Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) * forward).normalized;
            }

            // align the player with the movement
            if(forward.sqrMagnitude > float.Epsilon) {
                transform.forward = forward;
            }

            base.AnimationMove(axes, dt);
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }

            if(!ControllerData.AllowAirControl && !IsGrounded) {
                return;
            }

            _isRunning = axes.sqrMagnitude >= ControllerData.RunThresholdSquared;

            Vector3 speed = axes * ControllerData.MoveSpeed;
            Quaternion rotation = null != Owner.Viewer ? Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) : transform.localRotation;
            Rigidbody.velocity = rotation * new Vector3(speed.x, Rigidbody.velocity.y, speed.y);
        }

        public virtual void Jump(bool force=false)
        {
            if(!Owner.CanMove) {
                return;
            }

            if(!force && !IsGrounded) {
                return;
            }

            float gravity = -Physics.gravity.y + ControllerData.FallSpeedAdjustment;
            Vector3 velocity = Vector3.up * Mathf.Sqrt(ControllerData.JumpHeight * 2.0f * gravity);
            Rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        }
#endregion

        private IEnumerator RaycastRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(RaycastRoutineRate);
            while(true) {
                UpdateIsGrounded();

                yield return wait;
            }
        }

#region Grounded Check
        protected bool CheckIsGrounded(Vector3 center)
        {
            return Physics.CheckSphere(center, _groundCheckRadius, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore);
        }

        private void UpdateIsGrounded()
        {
            Profiler.BeginSample("ThirdPersonController.UpdateIsGrounded");
            try {
                _isGrounded = CheckIsGrounded(GroundCheckCenter);
            } finally {
                Profiler.EndSample();
            }
        }
#endregion

        private void FudgeVelocity(float dt)
        {
            Vector3 adjustedVelocity = Rigidbody.velocity;

            // do some fudging to jumping/falling so it feels better
            if(!IsGrounded) {
                float adjustment = ControllerData.FallSpeedAdjustment * dt;
                adjustedVelocity.y -= adjustment;
            }

            // apply terminal velocity
            if(adjustedVelocity.y < -ControllerData.TerminalVelocity) {
                adjustedVelocity.y = -ControllerData.TerminalVelocity;
            }

            Rigidbody.velocity = adjustedVelocity;
        }
    }
}
