using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Players
{
    public sealed class FlightController : Core.Actors.ActorController
    {
#region Physics
        [SerializeField]
        [ReadOnly]
        private Vector3 _bankForce;

        public Vector3 BankForce => _bankForce;

        public float Speed => PartyParrotManager.Instance.IsPaused ? _pauseState.Velocity.magnitude : Rigidbody.velocity.magnitude;

        public float Altitude => Owner.GameObject.transform.position.y;
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

        public void RotateModel(Vector3 axes, float dt)
        {
            Quaternion rotation = Owner.Model.transform.localRotation;
            Vector3 eulerAngles = Owner.Model.transform.localEulerAngles;

            Vector3 targetEuler = new Vector3();

            targetEuler.z = axes.x;
            targetEuler.x = axes.y;

            Quaternion targetRotation = Quaternion.Euler(targetEuler);
            rotation = Quaternion.Lerp(rotation, targetRotation, dt);

            Owner.Model.transform.localRotation = rotation;
        }

#region Movement
        public void Turn(Vector3 axes, float dt)
        {
#if true
            float turnSpeed = axes.x;
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

        public void Move(Vector3 axes, float dt)
        {
            float attackAngle = axes.y;
            Vector3 attackVector = Quaternion.AngleAxis(attackAngle, Vector3.right) * Vector3.forward;

/*
            Rigidbody.AddRelativeForce(attackVector * Owner.Bird.Type.Physics.LinearThrust);

            if(Owner.State.IsBraking) {
                Rigidbody.AddRelativeForce(Vector3.forward * -Owner.Bird.Type.Physics.BrakeThrust * Owner.State.BrakeAmount);
            }

            if(Owner.State.IsBoosting) {
                Rigidbody.AddRelativeForce(Vector3.forward * Owner.Bird.Type.Physics.BoostThrust * Owner.State.BoostAmount);
            }
*/

            // lift if we're not falling
            if(axes.y >= 0.0f) {
                Rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
            }

/*
            // cap our fall speed
            if(Rigidbody.velocity.y < -Owner.Bird.Type.Physics.TerminalVelocity) {
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, -Owner.Bird.Type.Physics.TerminalVelocity, Rigidbody.velocity.z);
            }
*/
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
