using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Players
{
    public class FlightController : ActorController
    {
#region Physics
        [SerializeField]
        [ReadOnly]
        private Vector3 _bankForce;

        public Vector3 BankForce => _bankForce;

        public float Speed => Owner.CanMove ? 0.0f : (PartyParrotManager.Instance.IsPaused ? _pauseState.Velocity.magnitude : Rigidbody.velocity.magnitude);

        public float Altitude => Owner.GameObject.transform.position.y;
#endregion

#region Movement Params
        [SerializeField]
        private float _maxAttackAngle = 45.0f;

        public float MaxAttackAngle => _maxAttackAngle;

        [SerializeField]
        private float _maxBankAngle = 45.0f;

        public float MaxBankAngle => _maxBankAngle;

        [SerializeField]
        private float _rotationAnimationSpeed = 5.0f;

        public float RotationAnimationSpeed => _rotationAnimationSpeed;

        [SerializeField]
        private float _linearThrust = 10.0f;

        public float LinearThrust => _linearThrust;

        [SerializeField]
        private float _turnSpeed = 10.0f;

        public float TurnSpeed => _turnSpeed;

        [SerializeField]
        private float _terminalVelocity = 10.0f;

        public float TerminalVelocity => _terminalVelocity;
#endregion

        [SerializeField]
        [ReadOnly]
        private PauseState _pauseState;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitRigidbody();

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        private void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }

        private void Update()
        {
#if DEBUG
            CheckForDebug();
#endif
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.angularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Rigidbody.velocity);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _bankForce);
        }
#endregion

        private void InitRigidbody()
        {
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
            Rigidbody.freezeRotation = true;
            Rigidbody.detectCollisions = true;
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            Rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        public void InitPhysics(float mass, float drag, float angularDrag)
        {
            Rigidbody.mass = mass;
            Rigidbody.drag = drag;
            Rigidbody.angularDrag = angularDrag;
        }

        public void Redirect(Vector3 velocity)
        {
            Debug.Log($"Redirecting player {Owner.Id}: {velocity}");

            // unwind all of the rotations
            Owner.Model.transform.localRotation = Quaternion.Euler(0.0f, Owner.Model.transform.localEulerAngles.y, 0.0f);
            Rigidbody.transform.rotation = Quaternion.Euler(0.0f, Rigidbody.transform.eulerAngles.y, 0.0f);

            // stop moving
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            // move in an orderly fashion!
            Rigidbody.velocity = velocity;
        }

#region Input Handling
#if UNITY_EDITOR
        private void CheckForDebug()
        {
            if(Keyboard.current[Key.B].isPressed) {
                Rigidbody.angularVelocity = Vector3.zero;
                Rigidbody.velocity = Vector3.zero;
            }
        }
#endif
#endregion

        public override void RotateModel(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }

            Quaternion rotation = Owner.Model.transform.localRotation;

            Vector3 targetEuler = new Vector3();
            targetEuler.z = axes.x * -MaxBankAngle;
            targetEuler.x = axes.y * -MaxAttackAngle;

            Quaternion targetRotation = Quaternion.Euler(targetEuler);
            rotation = Quaternion.Lerp(rotation, targetRotation, RotationAnimationSpeed * dt);

            Owner.Model.transform.localRotation = rotation;
        }

#region Movement
        public override void Turn(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }

#if true
            float turnSpeed = TurnSpeed * axes.x;
            Quaternion rotation = Quaternion.AngleAxis(turnSpeed * dt, Vector3.up);
            Rigidbody.MoveRotation(Rigidbody.rotation * rotation);
#else
            // TODO: this only works if Y rotatoin is unconstrained
            // it also breaks because the model rotates :(
            const float AngularThrust = 0.5f;
            Rigidbody.AddRelativeTorque(Vector3.up * AngularThrust * axes.x);
#endif

            // adding a force opposite our current x velocity should help stop us drifting
            Vector3 relativeVelocity = transform.InverseTransformDirection(Rigidbody.velocity);
            _bankForce = -relativeVelocity.x * Rigidbody.angularDrag * transform.right;
            Rigidbody.AddForce(_bankForce);
        }

        public override void Move(Vector3 axes, float dt)
        {
            if(!Owner.CanMove) {
                return;
            }

            float attackAngle = axes.y * -MaxAttackAngle;
            Vector3 attackVector = Quaternion.AngleAxis(attackAngle, Vector3.right) * Vector3.forward;
            Rigidbody.AddRelativeForce(attackVector * LinearThrust);

            // lift if we're not falling
            if(axes.y >= 0.0f) {
                Rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
            }

            // cap our fall speed
            if(Rigidbody.velocity.y < -TerminalVelocity) {
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, -TerminalVelocity, Rigidbody.velocity.z);
            }
        }
#endregion

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(PartyParrotManager.Instance.IsPaused) {
                _pauseState.Save(Rigidbody);
            } else {
                _pauseState.Restore(Rigidbody);
            }
            Rigidbody.isKinematic = PartyParrotManager.Instance.IsPaused;
        }
#endregion
    }
}
