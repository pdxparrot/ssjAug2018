using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors.ControllerComponents;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Effects;

using UnityEngine;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterActorController : ActorController
    {
        [SerializeField]
        private CharacterActorControllerData _controllerData;

        public CharacterActorControllerData ControllerData => _controllerData;

        public CapsuleCollider Capsule => (CapsuleCollider)Owner.Collider;

        [Space(10)]

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("How often to run raycast checks, in seconds")]
        private float _raycastRoutineRate = 0.1f;

        public float RaycastRoutineRate => _raycastRoutineRate;

        [Space(10)]

#region Ground Check
        [Header("Ground Check")]

        private RaycastHit[] _groundCheckHits = new RaycastHit[4];

        [SerializeField]
        [ReadOnly]
        private bool _didGroundCheckCollide;

        public bool DidGroundCheckCollide => _didGroundCheckCollide;

        [SerializeField]
        [ReadOnly]
        private Vector3 _groundCheckNormal;

        [SerializeField]
        [ReadOnly]
        private float _groundCheckMinDistance;

        [SerializeField]
        [ReadOnly]
        private bool _isGrounded;

        public bool IsGrounded { get { return _isGrounded; } protected set { _isGrounded = value; } }

        public bool IsFalling => !IsGrounded && !IsSliding && Rigidbody.velocity.y < 0.0f;

        private float GroundCheckRadius => Capsule.radius - 0.1f;

        protected Vector3 GroundCheckCenter => transform.position + (GroundCheckRadius * Vector3.up);
#endregion

        [Space(10)]

#region Slope Check
        [Header("Slope Check")]

        [SerializeField]
        [ReadOnly]
        private float _groundSlope;

        [SerializeField]
        [ReadOnly]
        private bool _isSliding;

        public bool IsSliding => _isSliding;
#endregion

        [Space(10)]

#region Effects
        [Header("Effects")]

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _groundedEffect;
#endregion

        private CharacterActorControllerComponent[] _components;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _components = GetComponents<CharacterActorControllerComponent>();

            InitRigidbody();

            StartCoroutine(RaycastRoutine());
        }

        protected override void Update()
        {
            base.Update();

            Owner.Animator.SetBool(ControllerData.FallingParam, IsFalling);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            float dt = Time.fixedDeltaTime;

            FudgeVelocity(dt);

            // turn off gravity if we're grounded and not moving and not sliding
            // this should stop us sliding down slopes we shouldn't slide down
            Rigidbody.useGravity = !IsGrounded || IsMoving || IsSliding;
        }

        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(Rigidbody.position, Rigidbody.position + Rigidbody.angularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Rigidbody.position, Rigidbody.position + Rigidbody.velocity);

            Gizmos.color = IsGrounded ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (ControllerData.GroundedEpsilon * Vector3.down), GroundCheckRadius);

            Gizmos.color = DidGroundCheckCollide ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (ControllerData.GroundCheckLength * Vector3.down), GroundCheckRadius);
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
            if(!CanMove) {
                return;
            }

            Vector3 forward = new Vector3(axes.x, 0.0f, axes.y);

            // align the movement with the camera
            if(null != Owner.Viewer) {
                forward = (Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) * forward).normalized;
            }

            // align the player with the movement
            if(forward.sqrMagnitude > float.Epsilon) {
                transform.forward = forward;
            }

            Owner.Animator.SetFloat(ControllerData.MoveXAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.x) : 0.0f);
            Owner.Animator.SetFloat(ControllerData.MoveZAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.y) : 0.0f);
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(RunOnComponents(c => c.OnPhysicsMove(axes, dt))) {
                return;
            }

            if(!ControllerData.AllowAirControl && IsFalling) {
                return;
            }

            DefaultPhysicsMove(axes, ControllerData.MoveSpeed, dt);
        }

        public void DefaultPhysicsMove(Vector3 axes, float speed, float dt)
        {
            if(!CanMove) {
                return;
            }

            Vector3 fixedAxes = new Vector3(axes.x, 0.0f, axes.y);

            // prevent moving up slopes we can't move up
            if(_groundSlope >= ControllerData.SlopeLimit) {
                float dp = Vector3.Dot(transform.forward, _groundCheckNormal);
                if(dp < 0.0f) {
                    fixedAxes.z = 0.0f;
                }
            }

            Vector3 velocity = fixedAxes * speed;
            Quaternion rotation = null != Owner.Viewer ? Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) : Rigidbody.rotation;
            velocity = rotation * velocity;
            velocity.y = Rigidbody.velocity.y;

            if(Rigidbody.isKinematic) {
                Rigidbody.MovePosition(Rigidbody.position + velocity * dt);
            } else {
                Rigidbody.velocity = velocity;
            }
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

            // force physics to a sane state for the first frame of the jump
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
            _didGroundCheckCollide = _isGrounded = false;

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
        protected bool CheckIsGrounded(out float minDistance)
        {
            minDistance = float.MaxValue;

            Vector3 origin = GroundCheckCenter;

            int hitCount = Physics.SphereCastNonAlloc(origin, GroundCheckRadius, Vector3.down, _groundCheckHits, ControllerData.GroundCheckLength, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore);
            if(hitCount < 1) {
                // no slope if not grounded
                _groundSlope = 0;
                return false;
            }

            // figure out the slope of whatever we hit
            _groundCheckNormal = Vector3.zero;
            for(int i=0; i<hitCount; ++i) {
                _groundCheckNormal += _groundCheckHits[i].normal;
                minDistance = Mathf.Min(minDistance, _groundCheckHits[i].distance);
            }
            _groundCheckNormal /= hitCount;

            _groundSlope = Vector3.Angle(Vector3.up, _groundCheckNormal);

            return true;
        }

        private void UpdateIsGrounded()
        {
            Profiler.BeginSample("Character.UpdateIsGrounded");
            try {
                bool wasGrounded = IsGrounded;

                _didGroundCheckCollide = CheckIsGrounded(out _groundCheckMinDistance);

                if(Rigidbody.isKinematic) {
                    // something else is handling this case?
                } else {
                    _isGrounded = _didGroundCheckCollide && _groundCheckMinDistance < ControllerData.GroundedEpsilon;
                }

                // if we're on a slope, we're sliding down it
                _isSliding = _groundSlope >= ControllerData.SlopeLimit;

                if(!wasGrounded && IsGrounded) {
                    Owner.Animator.SetTrigger(ControllerData.GroundedParam);
                    if(null != _groundedEffect) {
                        _groundedEffect.Trigger();
                    }
                }
            } finally {
                Profiler.EndSample();
            }
        }
#endregion

        private void FudgeVelocity(float dt)
        {
            Vector3 adjustedVelocity = Rigidbody.velocity;

            if(IsGrounded && !IsMoving) {
                // prevent any weird ground adjustment shenanigans
                // when we're grounded and not moving
                adjustedVelocity.y = 0.0f;
            } else if(Rigidbody.useGravity) {
                // do some fudging to jumping/falling so it feels better
                float adjustment = ControllerData.FallSpeedAdjustment * dt;
                adjustedVelocity.y -= adjustment;

                // apply terminal velocity
                if(adjustedVelocity.y < -ControllerData.TerminalVelocity) {
                    adjustedVelocity.y = -ControllerData.TerminalVelocity;
                }
            }

            Rigidbody.velocity = adjustedVelocity;
        }
    }
}
