using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.Actors;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerDriver : GamepadDriver
    {
        public Player Player => (Player)Owner;

        protected override bool CanDrive => base.CanDrive && Player.isLocalPlayer;

#region Unity Lifecycle
        private void OnDestroy()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.pause.performed -= OnPause;
                InputManager.Instance.Controls.game.move.performed -= OnMove;
                InputManager.Instance.Controls.game.look.performed -= OnLook;
                InputManager.Instance.Controls.game.jump.performed -= OnJump;
                InputManager.Instance.Controls.game.grab.performed -= OnGrab;
                InputManager.Instance.Controls.game.drop.performed -= OnDrop;
            }
        }
#endregion

        public override void Initialize(IActor owner, ActorController controller)
        {
            base.Initialize(owner, controller);

            if(CanDrive) {
                InputManager.Instance.Controls.game.pause.performed += OnPause;
                InputManager.Instance.Controls.game.move.performed += OnMove;
                InputManager.Instance.Controls.game.look.performed += OnLook;
                InputManager.Instance.Controls.game.jump.performed += OnJump;
                InputManager.Instance.Controls.game.grab.performed += OnGrab;
                InputManager.Instance.Controls.game.drop.performed += OnDrop;
            }
        }

#region Event Handlers
        private void OnPause(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

Debug.Log("pause");
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Vector2 axes = ctx.ReadValue<Vector2>();
            LastMoveAxes = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Vector2 axes = ctx.ReadValue<Vector2>();
            Player.FollowTarget.LookAxis = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.Jump();
        }

        private void OnGrab(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.Grab();
        }

        private void OnDrop(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.Drop();
        }
#endregion
    }
}
