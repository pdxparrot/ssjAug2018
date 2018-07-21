using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
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

        private Gamepad _gamepad;

#region Unity Lifecycle
        private void Awake()
        {
            // TODO: acquire gamepad in a better way
            // also need to watch for connection state changes
            _gamepad = Gamepad.current;
        }

        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused || null == _gamepad) {
                return;
            }

            Vector2 axes = _gamepad.leftStick.ReadValue();
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
    }
}
