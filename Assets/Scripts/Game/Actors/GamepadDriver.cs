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

#region Unity Lifecycle
        private void Awake()
        {
// acquire a gamepad
        }

        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            Vector2 axes = Gamepad.current.leftStick.ReadValue();
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
