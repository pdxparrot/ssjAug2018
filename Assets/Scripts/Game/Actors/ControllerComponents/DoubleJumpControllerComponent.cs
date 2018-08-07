using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class DoubleJumpControllerComponent : CharacterActorControllerComponent
    {
        [SerializeField]
        [ReadOnly]
        private int _doubleJumpCount;

        private bool CanDoubleJump => !Controller.IsGrounded && (Controller.ControllerData.DoubleJumpCount < 0 || _doubleJumpCount < Controller.ControllerData.DoubleJumpCount);

#region Unity Lifecycle
        private void Update()
        {
            if(Controller.IsGrounded) {
                _doubleJumpCount = 0;
            }
        }
#endregion

        public void Reset()
        {
            _doubleJumpCount = 0;
        }

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(!(action is JumpControllerComponent.JumpAction)) {
                return false;
            }

            if(!CanDoubleJump) {
                return false;
            }

            Controller.Jump(Controller.ControllerData.DoubleJumpHeight, Controller.ControllerData.DoubleJumpParam);

            _doubleJumpCount++;
            return true;
        }
    }
}
