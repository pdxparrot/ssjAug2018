using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Camera;

using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(FollowTarget))]
    public sealed class ServerSpectator : MonoBehaviour
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

        public FollowTarget FollowTarget { get; private set; }

        [CanBeNull]
        private ServerSpectatorViewer _viewer;

#region Unity Lifecycle
        private void Awake()
        {
            FollowTarget = GetComponent<FollowTarget>();

            _viewer = ViewerManager.Instance.AcquireViewer<ServerSpectatorViewer>();
            if(null != _viewer) {
                _viewer.Initialize(this);
            }

            InputManager.Instance.Controls.game.pause.performed += OnPause;

            InputManager.Instance.Controls.game.moveforward.started += OnMoveForward;
            InputManager.Instance.Controls.game.moveforward.performed += OnMoveForwardStop;
            InputManager.Instance.Controls.game.movebackward.started += OnMoveBackward;
            InputManager.Instance.Controls.game.movebackward.performed += OnMoveBackwardStop;
            InputManager.Instance.Controls.game.moveleft.started += OnMoveLeft;
            InputManager.Instance.Controls.game.moveleft.performed += OnMoveLeftStop;
            InputManager.Instance.Controls.game.moveright.started += OnMoveRight;
            InputManager.Instance.Controls.game.moveright.performed += OnMoveRightStop;
            InputManager.Instance.Controls.game.jump.started += OnMoveUp;
            InputManager.Instance.Controls.game.jump.performed += OnMoveUpStop;
            InputManager.Instance.Controls.game.movedown.started += OnMoveDown;
            InputManager.Instance.Controls.game.movedown.performed += OnMoveDownStop;

            InputManager.Instance.Controls.game.look.started += OnLook;
            InputManager.Instance.Controls.game.look.performed += OnLook;
            InputManager.Instance.Controls.game.look.cancelled += OnLookStop;
        }

        private void OnDestroy()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.pause.performed -= OnPause;

                InputManager.Instance.Controls.game.moveforward.started -= OnMoveForward;
                InputManager.Instance.Controls.game.moveforward.performed -= OnMoveForwardStop;
                InputManager.Instance.Controls.game.movebackward.started -= OnMoveBackward;
                InputManager.Instance.Controls.game.movebackward.performed -= OnMoveBackwardStop;
                InputManager.Instance.Controls.game.moveleft.started -= OnMoveLeft;
                InputManager.Instance.Controls.game.moveleft.performed -= OnMoveLeftStop;
                InputManager.Instance.Controls.game.moveright.started -= OnMoveRight;
                InputManager.Instance.Controls.game.moveright.performed -= OnMoveRightStop;
                InputManager.Instance.Controls.game.jump.started -= OnMoveUp;
                InputManager.Instance.Controls.game.jump.performed -= OnMoveUpStop;
                InputManager.Instance.Controls.game.movedown.started -= OnMoveDown;
                InputManager.Instance.Controls.game.movedown.performed -= OnMoveDownStop;

                InputManager.Instance.Controls.game.look.started -= OnLook;
                InputManager.Instance.Controls.game.look.performed -= OnLook;
                InputManager.Instance.Controls.game.look.cancelled -= OnLookStop;
            }

            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(_viewer);
            }
            _viewer = null;
        }

        private void FixedUpdate()
        {
            float dt = Time.deltaTime;

            FollowTarget.LastLookAxes = Vector3.Lerp(FollowTarget.LastLookAxes, _lastControllerLook, dt * 20.0f);

            Quaternion rotation = null != _viewer ? Quaternion.AngleAxis(_viewer.transform.localEulerAngles.y, Vector3.up) : transform.rotation;
            transform.position = Vector3.Lerp(transform.position, transform.position + (rotation * _lastControllerMove), dt * 20.0f);
        }
#endregion

        private bool IsOurDevice(InputAction.CallbackContext ctx)
        {
            // no input unless we have focus
            if(!Application.isFocused) {
                return false;
            }

            // TODO: this probably doesn't handle multiple keyboards/mice
            return Keyboard.current == ctx.control.device || Mouse.current == ctx.control.device;
        }

#region Event Handlers

        private void OnPause(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            PartyParrotManager.Instance.TogglePause();
        }

#region Move
        private void OnMoveForward(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, _lastControllerMove.y, 1.0f);
        }

        private void OnMoveForwardStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, _lastControllerMove.y, 0.0f);
        }

        private void OnMoveBackward(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, _lastControllerMove.y, -1.0f);
        }

        private void OnMoveBackwardStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, _lastControllerMove.y, 0.0f);
        }

        private void OnMoveLeft(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(-1.0f, _lastControllerMove.y, _lastControllerMove.z);
        }

        private void OnMoveLeftStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(0.0f, _lastControllerMove.y, _lastControllerMove.z);
        }

        private void OnMoveRight(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(1.0f, _lastControllerMove.y, _lastControllerMove.z);
        }

        private void OnMoveRightStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(0.0f, _lastControllerMove.y, _lastControllerMove.z);
        }

        private void OnMoveUp(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 1.0f, _lastControllerMove.z);
        }

        private void OnMoveUpStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 0.0f, _lastControllerMove.z);
        }

        private void OnMoveDown(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, -1.0f, _lastControllerMove.z);
        }

        private void OnMoveDownStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 0.0f, _lastControllerMove.z);
        }
#endregion

#region Look
        private void OnLook(InputAction.CallbackContext ctx)
        {
            bool isMouse = ctx.control.device is Mouse;
            if(!IsOurDevice(ctx)) {
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
            if(!IsOurDevice(ctx)) {
                return;
            }

            _lastControllerLook = Vector3.zero;
            FollowTarget.LastLookAxes = _lastControllerLook;
        }
#endregion

#endregion
    }
}
