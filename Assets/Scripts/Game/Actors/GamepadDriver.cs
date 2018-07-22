using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Actors
{
    public class GamepadDriver : ActorDriver
    {
        [SerializeField]
        [ReadOnly]
        private Vector3 _lastMoveAxes;

        [CanBeNull]
        private Gamepad _gamepad;

        public bool HasGamepad => null != _gamepad;

#region Unity Lifecycle
        private void Awake()
        {
            InputManager.Instance.AcquireGamepad(OnAcquireGamepad, OnGamepadDisconnect);
        }

        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            Vector2 axes = _gamepad?.leftStick.ReadValue() ?? new Vector2();
            _lastMoveAxes = new Vector3(axes.x, 0.0f, axes.y);

            float dt = Time.deltaTime;

            Controller.RotateModel(_lastMoveAxes, dt);
        }

        private void FixedUpdate()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.fixedDeltaTime;

            //Controller.Turn(_lastMoveAxes, dt);
            Controller.Move(_lastMoveAxes, dt);
        }
#endregion

#region Event Handlers
        private void OnAcquireGamepad(Gamepad gamepad)
        {
            _gamepad = gamepad;
        }

        private void OnGamepadDisconnect(Gamepad gamepad)
        {
            _gamepad = null;
        }
#endregion
    }
}
