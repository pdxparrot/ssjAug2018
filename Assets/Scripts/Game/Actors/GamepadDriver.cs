using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Actors
{
    public abstract class GamepadDriver : ActorDriver
    {
        [SerializeField]
        [ReadOnly]
        private int _gamepadId;

#region TODO: move to driver config
        [SerializeField]
        private float _xAxisDeadZone = 0.01f;

        [SerializeField]
        private float _yAxisDeadZone = 0.01f;
#endregion

        [CanBeNull]
        private Gamepad _gamepad;

        protected Gamepad Gamepad => _gamepad;

        public bool HasGamepad => null != _gamepad;

#region Unity Lifecycle
        protected virtual void OnDestroy()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.ReleaseGamepad(_gamepadId);
                _gamepadId = 0;
                _gamepad = null;
            }
        }
#endregion

        public override void Initialize(IActor owner, ActorController controller)
        {
            base.Initialize(owner, controller);

            InputManager.Instance.AcquireGamepad(OnAcquireGamepad, OnGamepadDisconnect);
        }

        protected bool IsOurGamepad(InputAction.CallbackContext ctx)
        {
            return ctx.control.device == _gamepad;
        }

        protected Vector2 ApplyDeadZone(Vector2 axis)
        {
            return new Vector2()
            {
                x = axis.x < -_xAxisDeadZone || axis.x > _xAxisDeadZone ? axis.x : 0.0f,
                y = axis.y < -_yAxisDeadZone || axis.y > _yAxisDeadZone ? axis.y : 0.0f
            };
        }

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
