using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerDriver : ActorDriver
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

        private PlayerController PlayerController => (PlayerController)Controller;

        private Player Player => PlayerController.Player;

        protected override bool CanDrive => base.CanDrive && Player.IsLocalActor;

        private bool _enableMouseLook = !Application.isEditor;

        private GamepadListener _gamepadListener;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        private void Update()
        {
            if(!Player.IsLocalActor) {
                return;
            }

            float dt = Time.deltaTime;

            Controller.LastMoveAxes = Vector3.Lerp(Controller.LastMoveAxes, _lastControllerMove, dt * PlayerManager.Instance.PlayerData.MovementLerpSpeed);
            Player.FollowTarget.LastLookAxes = Vector3.Lerp(Player.FollowTarget.LastLookAxes, _lastControllerLook, dt * PlayerManager.Instance.PlayerData.LookLerpSpeed);
        }

        private void OnDestroy()
        {
            Destroy(_gamepadListener);
            _gamepadListener = null;

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
        }
#endregion

        public void Initialize()
        {
            if(!Player.IsLocalActor) {
                return;
            }

            _gamepadListener = gameObject.AddComponent<GamepadListener>();

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

            InitDebugMenu();
        }

        private bool IsOurDevice(InputAction.CallbackContext ctx)
        {
            // no input unless we have focus
            if(!Application.isFocused) {
                return false;
            }

            return _gamepadListener.IsOurGamepad(ctx) ||
                // ignore keyboard/mouse while the debug menu is open
                // TODO: this probably doesn't handle multiple keyboards/mice
                (!DebugMenuManager.Instance.Enabled && (Keyboard.current == ctx.control.device || Mouse.current == ctx.control.device));
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
            Controller.LastMoveAxes = _lastControllerMove;
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
            Controller.LastMoveAxes = _lastControllerMove;
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
            Controller.LastMoveAxes = _lastControllerMove;
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
            Controller.LastMoveAxes = _lastControllerMove;
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
            Controller.LastMoveAxes = _lastControllerMove;
        }
#endregion

#region Look
        private void OnLook(InputAction.CallbackContext ctx)
        {
            bool isMouse = ctx.control.device is Mouse;
            if(!IsOurDevice(ctx) || !CanDrive || (isMouse && !_enableMouseLook)) {
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

#region Aim
        private void OnAimStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionStarted(AimControllerComponent.AimAction.Default);
        }

        private void OnAim(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionCancelled(AimControllerComponent.AimAction.Default);
        }
#endregion

#region Throw
        private void OnThrowMailStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionStarted(ThrowControllerComponent.ThrowMailAction.Default);
        }

        private void OnThrowMail(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionPerformed(ThrowControllerComponent.ThrowMailAction.Default);
        }

        private void OnThrowSnowballStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionStarted(ThrowControllerComponent.ThrowSnowballAction.Default);
        }

        private void OnThrowSnowball(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionPerformed(ThrowControllerComponent.ThrowSnowballAction.Default);
        }
#endregion

#endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Player {Player.name} Driver");
            _debugMenuNode.RenderContentsAction = () => {
                _invertLookY = GUILayout.Toggle(_invertLookY, "Invert Look Y");
                /*GUILayout.BeginHorizontal();
                    GUILayout.Label("Mouse Sensitivity:");
                    _mouseSensitivity = GUIUtils.FloatField(_mouseSensitivity);
                GUILayout.EndHorizontal();*/

                if(Application.isEditor) {
                    _enableMouseLook = GUILayout.Toggle(_enableMouseLook, "Enable Mouse Look");
                }
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
