using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.Actors;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerDriver : GamepadDriver
    {
        [SerializeField]
        private bool _invertLookY;

        public Player Player => (Player)Owner;

        protected override bool CanDrive => base.CanDrive && Player.isLocalPlayer;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();
        }

        protected override void Update()
        {
            // TODO: until we have a way to know when the player stops pushing on the stick
            // through the InputSystem, this is the best we can do here
            // https://forum.unity.com/threads/gamepad-joystick-movement-question.542629/
            if(null != Gamepad) {
                Vector2 moveAxes = ApplyDeadZone(Gamepad.leftStick.ReadValue());
                LastMoveAxes = new Vector3(moveAxes.x, moveAxes.y, 0.0f);

                Vector2 lookAxes = ApplyDeadZone(Gamepad.rightStick.ReadValue());
                lookAxes.y *= _invertLookY ? -1 : 1;
                Player.FollowTarget.LastLookAxes = new Vector3(lookAxes.x, lookAxes.y, 0.0f);
            }

            base.Update();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();

            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.pause.performed -= OnPause;

                /*InputManager.Instance.Controls.game.move.started -= OnMove;
                InputManager.Instance.Controls.game.move.performed -= OnMove;
                InputManager.Instance.Controls.game.move.cancelled -= OnMoveStop;

                InputManager.Instance.Controls.game.look.started -= OnLook;
                InputManager.Instance.Controls.game.look.performed -= OnLook;
                InputManager.Instance.Controls.game.look.cancelled -= OnLookStop;*/

                InputManager.Instance.Controls.game.jump.started -= OnJumpStart;
                InputManager.Instance.Controls.game.jump.performed -= OnJump;
                InputManager.Instance.Controls.game.hover.started -= OnHoverStart;
                InputManager.Instance.Controls.game.hover.performed -= OnHover;

                InputManager.Instance.Controls.game.grab.performed -= OnGrab;
                InputManager.Instance.Controls.game.drop.performed -= OnDrop;

                InputManager.Instance.Controls.game.aim.started -= OnAimStart;
                InputManager.Instance.Controls.game.aim.performed -= OnAim;
                InputManager.Instance.Controls.game.throwmail.started -= OnThrowMailStart;
                InputManager.Instance.Controls.game.throwmail.performed -= OnThrowMail;
                InputManager.Instance.Controls.game.throwsnowball.started -= OnThrowSnowballStart;
                InputManager.Instance.Controls.game.throwsnowball.performed -= OnThrowSnowball;
            }

            base.OnDestroy();
        }
#endregion

        public override void Initialize(IActor owner, ActorController controller)
        {
            base.Initialize(owner, controller);

            if(CanDrive) {
                InputManager.Instance.Controls.game.pause.performed += OnPause;

                /*InputManager.Instance.Controls.game.move.started += OnMove;
                InputManager.Instance.Controls.game.move.performed += OnMove;
                InputManager.Instance.Controls.game.move.cancelled += OnMoveStop;

                InputManager.Instance.Controls.game.look.started += OnLook;
                InputManager.Instance.Controls.game.look.performed += OnLook;
                InputManager.Instance.Controls.game.look.cancelled += OnLookStop;*/

                InputManager.Instance.Controls.game.jump.started += OnJumpStart;
                InputManager.Instance.Controls.game.jump.performed += OnJump;
                InputManager.Instance.Controls.game.hover.started += OnHoverStart;
                InputManager.Instance.Controls.game.hover.performed += OnHover;

                InputManager.Instance.Controls.game.grab.performed += OnGrab;
                InputManager.Instance.Controls.game.drop.performed += OnDrop;

                InputManager.Instance.Controls.game.aim.started += OnAimStart;
                InputManager.Instance.Controls.game.aim.performed += OnAim;
                InputManager.Instance.Controls.game.throwmail.started += OnThrowMailStart;
                InputManager.Instance.Controls.game.throwmail.performed += OnThrowMail;
                InputManager.Instance.Controls.game.throwsnowball.started += OnThrowSnowballStart;
                InputManager.Instance.Controls.game.throwsnowball.performed += OnThrowSnowball;
            }
        }

#region Event Handlers
        private void OnPause(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx)) {
                return;
            }

            PartyParrotManager.Instance.TogglePause();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Vector2 axes = ApplyDeadZone(ctx.ReadValue<Vector2>());
            LastMoveAxes = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnMoveStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            LastMoveAxes = new Vector3(0.0f, 0.0f, 0.0f);
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Vector2 axes = ApplyDeadZone(ctx.ReadValue<Vector2>());
            axes.y *= _invertLookY ? -1 : 1;

            Player.FollowTarget.LastLookAxes = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnLookStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.FollowTarget.LastLookAxes = new Vector3(0.0f, 0.0f, 0.0f);
        }

        private void OnJumpStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.JumpStart();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.Jump();
        }

        private void OnHoverStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.HoverStart();
        }

        private void OnHover(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.Hover();
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

        private void OnAimStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.StartAim();
        }

        private void OnAim(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.Aim();
        }

        private void OnThrowMailStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.StartThrowMail();
        }

        private void OnThrowMail(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ThrowMail();
        }

        private void OnThrowSnowballStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.StartThrowSnowball();
        }

        private void OnThrowSnowball(InputAction.CallbackContext ctx)
        {
            if(!IsOurGamepad(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ThrowSnowball();
        }
#endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Player {Player.name} Driver");
            _debugMenuNode.RenderContentsAction = () => {
                _invertLookY = GUILayout.Toggle(_invertLookY, "Invert Look Y");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
