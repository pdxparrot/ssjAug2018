using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class LongJumpControllerComponent : CharacterActorControllerComponent
    {
        [SerializeField]
        [ReadOnly]
        private bool _isHeld;

        [SerializeField]
        [ReadOnly]
        private float _heldSeconds;

        private bool CanLongJump => !_didLongJump && _heldSeconds >= Controller.ControllerData.LongJumpHoldSeconds;

        [SerializeField]
        [ReadOnly]
        private bool _didLongJump;

#region Unity Lifecycle
        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.deltaTime;

            if(!Controller.IsGrounded) {
                _isHeld = false;
                _heldSeconds = 0;
            }

            if(_isHeld) {
                _heldSeconds += dt;

                if(CanLongJump) {
                    Controller.Jump(Controller.ControllerData.LongJumpHeight, Controller.ControllerData.LongJumpParam);
                    _didLongJump = true;
                }
            }
        }
#endregion

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(!(action is JumpControllerComponent.JumpAction)) {
                return false;
            }

            if(!Controller.IsGrounded || Controller.IsSliding) {
                return false;
            }

            _isHeld = true;
            _heldSeconds = 0;
            _didLongJump = false;

            return true;
        }

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(!(action is JumpControllerComponent.JumpAction)) {
                return false;
            }

            _isHeld = false;
            _heldSeconds = 0;

            return _didLongJump;
        }
    }
}
