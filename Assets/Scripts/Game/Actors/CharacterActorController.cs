using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors.ControllerComponents;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors
{
    public class CharacterActorController : ActorController
    {
        [SerializeField]
        private CharacterActorControllerData _controllerData;

        public CharacterActorControllerData ControllerData => _controllerData;

        [Space(10)]

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("How often to run raycast checks, in seconds")]
        private float _raycastRoutineRate = 0.1f;

        public float RaycastRoutineRate => _raycastRoutineRate;

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

        public bool IsGrounded { get { return _isGrounded; } protected set { _isGrounded = value; } }

        [SerializeField]
        [ReadOnly]
        private bool _didGroundCheckCollide;

        public bool DidGroundCheckCollide => _didGroundCheckCollide;

        [SerializeField]
        [ReadOnly]
        private bool _isFalling;

        public bool IsFalling { get { return _isFalling; } protected set { _isFalling = value; } }

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

        private CharacterActorControllerComponent[] _components;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _components = GetComponents<CharacterActorControllerComponent>();

            InitRigidbody();
        }

        protected override void Update()
        {
            base.Update();

            _isRunning = Driver.LastMoveAxes.sqrMagnitude >= ControllerData.RunThresholdSquared;

            Owner.Animator.SetFloat(ControllerData.MoveXAxisParam, CanMove ? Mathf.Abs(Driver.LastMoveAxes.x) : 0.0f);
            Owner.Animator.SetFloat(ControllerData.MoveZAxisParam, CanMove ? Mathf.Abs(Driver.LastMoveAxes.y) : 0.0f);

            Owner.Animator.SetBool(ControllerData.FallingParam, IsFalling);
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

        public override void AnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(RunOnComponents(c => c.OnAnimationMove(axes, dt))) {
                return;
            }

            DefaultAnimationMove(axes, dt);
        }

        public void DefaultAnimationMove(Vector3 axes, float dt)
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
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(RunOnComponents(c => c.OnPhysicsMove(axes, dt))) {
                return;
            }

            if(!ControllerData.AllowAirControl && !IsGrounded) {
                return;
            }

            DefaultPhysicsMove(axes, dt);
        }

        public void DefaultPhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            Vector3 speed = axes * ControllerData.MoveSpeed;
            Quaternion rotation = null != Owner.Viewer ? Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) : Rigidbody.rotation;
            Vector3 velocity = rotation * new Vector3(speed.x, 0.0f, speed.y);
            velocity.y = Rigidbody.velocity.y;

            Rigidbody.velocity = velocity;
        }

#region Components
        [CanBeNull]
        public T GetControllerComponent<T>() where T: CharacterActorControllerComponent
        {
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    return tc;
                }
            }
            return null;
        }

        public void GetControllerComponents<T>(ICollection<T> components) where T: CharacterActorControllerComponent
        {
            components.Clear();
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    components.Add(tc);
                }
            }
        }

        private bool RunOnComponents(Func<CharacterActorControllerComponent, bool> f)
        {
            foreach(var component in _components) {
                if(f(component)) {
                    return true;
                }
            }
            return false;
        }
#endregion

#region Actions
        public virtual void ActionStarted(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            RunOnComponents(c => c.OnStarted(action));
        }

        public virtual void ActionPerformed(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            RunOnComponents(c => c.OnPerformed(action));
        }

        public virtual void ActionCancelled(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            RunOnComponents(c => c.OnCancelled(action));
        }
#endregion

        public void Jump(float height, string animationParam)
        {
            if(!CanMove) {
                return;
            }

            Rigidbody.isKinematic = false;

            // factor in fall speed adjust
            float gravity = -Physics.gravity.y + ControllerData.FallSpeedAdjustment;

            // v = sqrt(2gh)
            Vector3 velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);

            Rigidbody.velocity = velocity;

            Owner.Animator.SetTrigger(animationParam);
        }

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
            Profiler.BeginSample("Character.UpdateIsGrounded");
            try {
                bool wasGrounded = IsGrounded;

                _didGroundCheckCollide = CheckIsGrounded(GroundCheckCenter);
                if(!Rigidbody.isKinematic) {
                    _isGrounded = _didGroundCheckCollide;
                }

                if(!wasGrounded && IsGrounded) {
                    Owner.Animator.SetTrigger(ControllerData.GroundedParam);
                }
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
