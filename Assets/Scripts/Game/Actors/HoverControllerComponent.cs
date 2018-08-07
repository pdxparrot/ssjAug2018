using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors
{
    public sealed class HoverControllerComponent : CharacterActorControllerComponent
    {
        public class HoverAction : CharacterActorControllerAction
        {
            public static HoverAction Default = new HoverAction();
        }

        [SerializeField]
        [ReadOnly]
        private bool _isHeld;

        [SerializeField]
        [ReadOnly]
        private float _heldSeconds;

        private bool CanHover => _heldSeconds >= Controller.ControllerData.HoverHoldSeconds;

        [SerializeField]
        [ReadOnly]
        private float _hoverTimeSeconds;

        public float RemainingPercent => 1.0f - (_hoverTimeSeconds / Controller.ControllerData.HoverTimeSeconds);

        [SerializeField]
        [ReadOnly]
        private float _cooldownCountdown;

        private bool IsHoverCooldown => _cooldownCountdown > 0.0f;

        [SerializeField]
        [ReadOnly]
        private bool _isHovering;

        public bool IsHovering => _isHovering;

#region Unity Lifecycle
        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.deltaTime;

            if(_isHeld) {
                _heldSeconds += dt;
            }

            if(IsHovering) {
                _hoverTimeSeconds += dt;
                if(_hoverTimeSeconds >= Controller.ControllerData.HoverTimeSeconds) {
                    _hoverTimeSeconds = Controller.ControllerData.HoverTimeSeconds;
                    EnableHovering(false);
                }
            } else if(IsHoverCooldown) {
                _cooldownCountdown -= dt;
            } else if(CanHover) {
                EnableHovering(true);
            } else if(_hoverTimeSeconds > 0.0f) {
                _hoverTimeSeconds -= dt;
                if(_hoverTimeSeconds < 0.0f) {
                    _hoverTimeSeconds = 0.0f;
                }
            }
        }
#endregion

        public override bool OnPhysicsMove(Vector3 axes, float dt)
        {
            if(!IsHovering) {
                return false;
            }

            Vector3 acceleration = (Controller.ControllerData.HoverAcceleration + Controller.ControllerData.FallSpeedAdjustment) * Vector3.up;
            Controller.Rigidbody.AddForce(acceleration, ForceMode.Acceleration);

            Controller.DefaultPhysicsMove(axes, dt);

            return true;
        }

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(!(action is HoverAction)) {
                return false;
            }

            if(Controller.IsGrounded && !Controller.ControllerData.HoverWhenGrounded) {
                return false;
            }

            _isHeld = true;
            _heldSeconds = 0;

            return true;
        }

        public override bool OnCancelled(CharacterActorControllerAction action)
        {
            if(!(action is HoverAction)) {
                return false;
            }

            _isHeld = false;
            _heldSeconds = 0;

            EnableHovering(false);

            return true;
        }

        public void DisableHovering()
        {
            EnableHovering(false);
        }

        private void EnableHovering(bool enable)
        {
            bool wasHovering = IsHovering;
            _isHovering = enable;

            Controller.Owner.Animator.SetBool(Controller.ControllerData.HoverParam, enable);

            if(enable) {
                // stop all vertical movement immediately
                Controller.Rigidbody.velocity = new Vector3(Controller.Rigidbody.velocity.x, 0.0f, Controller.Rigidbody.velocity.z);
            } else if(wasHovering) {
                _cooldownCountdown = Controller.ControllerData.HoverCooldownSeconds;
            }
        }
    }
}
