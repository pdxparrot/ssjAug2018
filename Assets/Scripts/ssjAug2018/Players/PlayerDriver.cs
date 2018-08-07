using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerDriver : GamepadDriver
    {
        [SerializeField]
        private bool _invertLookY;

        [SerializeField]
        private float _mouseSensitivity = 0.5f;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastControllerMove;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastControllerLook;

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
            float dt = Time.deltaTime;

            LastMoveAxes = Vector3.Lerp(LastMoveAxes, _lastControllerMove, dt * PlayerManager.Instance.PlayerData.MovementLerpSpeed);
            Player.FollowTarget.LastLookAxes = Vector3.Lerp(Player.FollowTarget.LastLookAxes, _lastControllerLook, dt * PlayerManager.Instance.PlayerData.LookLerpSpeed);

            base.Update();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();

            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.pause.performed -= OnPause;

                InputManager.Instance.Controls.game.move.started -= OnMove;
                InputManager.Instance.Controls.game.move.performed -= OnMove;
                InputManager.Instance.Controls.game.move.cancelled -= OnMoveStop;

                InputManager.Instance.Controls.game.moveforward.started -= OnMoveForward;
                InputManager.Instance.Controls.game.moveforward.performed -= OnMoveForwardStop;
                InputManager.Instance.Controls.game.movebackward.started -= OnMoveBackward;
                InputManager.Instance.Controls.game.movebackward.performed -= OnMoveBackwardStop;
                InputManager.Instance.Controls.game.moveleft.started -= OnMoveLeft;
                InputManager.Instance.Controls.game.moveleft.performed -= OnMoveLeftStop;
                InputManager.Instance.Controls.game.moveright.started -= OnMoveRight;
                InputManager.Instance.Controls.game.moveright.performed -= OnMoveRightStop;

                InputManager.Instance.Controls.game.look.started -= OnLook;
                InputManager.Instance.Controls.game.look.performed -= OnLook;
                InputManager.Instance.Controls.game.look.cancelled -= OnLookStop;

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

            if(!Player.isLocalPlayer) {
                return;
            }

            InputManager.Instance.Controls.game.pause.performed += OnPause;

            InputManager.Instance.Controls.game.move.started += OnMove;
            InputManager.Instance.Controls.game.move.performed += OnMove;
            InputManager.Instance.Controls.game.move.cancelled += OnMoveStop;

            InputManager.Instance.Controls.game.moveforward.started += OnMoveForward;
            InputManager.Instance.Controls.game.moveforward.performed += OnMoveForwardStop;
            InputManager.Instance.Controls.game.movebackward.started += OnMoveBackward;
            InputManager.Instance.Controls.game.movebackward.performed += OnMoveBackwardStop;
            InputManager.Instance.Controls.game.moveleft.started += OnMoveLeft;
            InputManager.Instance.Controls.game.moveleft.performed += OnMoveLeftStop;
            InputManager.Instance.Controls.game.moveright.started += OnMoveRight;
            InputManager.Instance.Controls.game.moveright.performed += OnMoveRightStop;

            InputManager.Instance.Controls.game.look.started += OnLook;
            InputManager.Instance.Controls.game.look.performed += OnLook;
            InputManager.Instance.Controls.game.look.cancelled += OnLookStop;

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

        private bool IsOurDevice(InputAction.CallbackContext ctx)
        {
            // TODO: this probably doesn't handle multiple keyboards/mice
            return IsOurGamepad(ctx) || Keyboard.current == ctx.control.device || Mouse.current == ctx.control.device;
        }

#region Event Handlers
        private void OnPause(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            PartyParrotManager.Instance.TogglePause();
        }

#region Gamepad Move
        private void OnMove(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Vector2 axes = ctx.ReadValue<Vector2>();
            _lastControllerMove = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnMoveStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = Vector3.zero;
            LastMoveAxes = _lastControllerMove;
        }
#endregion

#region Keyboard Move
        private void OnMoveForward(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 1.0f, 0.0f);
        }

        private void OnMoveForwardStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 0.0f, 0.0f);
            LastMoveAxes = _lastControllerMove;
        }

        private void OnMoveBackward(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, -1.0f, 0.0f);
        }

        private void OnMoveBackwardStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 0.0f, 0.0f);
            LastMoveAxes = _lastControllerMove;
        }

        private void OnMoveLeft(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(-1.0f, _lastControllerMove.y, 0.0f);
        }

        private void OnMoveLeftStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(0.0f, _lastControllerMove.y, 0.0f);
            LastMoveAxes = _lastControllerMove;
        }

        private void OnMoveRight(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(1.0f, _lastControllerMove.y, 0.0f);
        }

        private void OnMoveRightStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(0.0f, _lastControllerMove.y, 0.0f);
            LastMoveAxes = _lastControllerMove;
        }
#endregion

#region Look
        private void OnLook(InputAction.CallbackContext ctx)
        {
            // TODO: mouse is disabled for now because it's annoying in the editor
            bool isMouse = ctx.control.device is Mouse;
            if(!IsOurDevice(ctx) || !CanDrive || isMouse) {
                return;
            }

            Vector2 axes = ctx.ReadValue<Vector2>();
            axes.y *= _invertLookY ? -1 : 1;

            if(isMouse) {
                axes *= _mouseSensitivity;
            }

            _lastControllerLook = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnLookStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerLook = Vector3.zero;
            Player.FollowTarget.LastLookAxes = _lastControllerLook;
        }
#endregion

#region Jump
        private void OnJumpStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionStarted(JumpControllerComponent.JumpAction.Default);
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionPerformed(JumpControllerComponent.JumpAction.Default);
        }
#endregion

#region Hover
        private void OnHoverStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionStarted(HoverControllerComponent.HoverAction.Default);
        }

        private void OnHover(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionCancelled(HoverControllerComponent.HoverAction.Default);
        }
#endregion

#region Grabbing / Dropping
        private void OnGrab(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionPerformed(ClimbingControllerComponent.GrabAction.Default);
        }

        private void OnDrop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionPerformed(ClimbingControllerComponent.ReleaseAction.Default);
        }
#endregion

        private void OnAimStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.StartAim();
        }

        private void OnAim(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.Aim();
        }

        private void OnThrowMailStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.StartThrowMail();
        }

        private void OnThrowMail(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ThrowMail();
        }

        private void OnThrowSnowballStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.StartThrowSnowball();
        }

        private void OnThrowSnowball(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
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
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Mouse Sensitivity:");
                    _mouseSensitivity = GUIUtils.FloatField(_mouseSensitivity);
                GUILayout.EndHorizontal();
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
