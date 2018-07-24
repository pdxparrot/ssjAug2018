using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Input;

using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Actors
{
    public class GamepadDriver : ActorDriver
    {
        [CanBeNull]
        private Gamepad _gamepad;

        protected Gamepad Gamepad => _gamepad;

        public bool HasGamepad => null != _gamepad;

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
