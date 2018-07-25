using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Input;

using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Actors
{
    public class GamepadDriver : ActorDriver
    {
        private int _gamepadId;

        [CanBeNull]
        private Gamepad _gamepad;

        protected Gamepad Gamepad => _gamepad;

        public bool HasGamepad => null != _gamepad;

#region Unity Lifecycle
        private void OnDestroy()
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
