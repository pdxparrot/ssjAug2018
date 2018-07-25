using JetBrains.Annotations;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Intro : pdxpartyparrot.Game.State.GameState
    {
        [SerializeField]
        private MainMenu _mainMenuState;

        private Viewer _viewer;

        private int _gamepadId;

        [CanBeNull]
        private Gamepad _gamepad;

        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();

            _viewer = ViewerManager.Instance.AcquireViewer();
            _viewer?.Set2D();
            _viewer?.EnableCamera(false);

            _gamepadId = InputManager.Instance.AcquireGamepad(OnAcquireGamepad, OnGamepadDisconnect);
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            GameStateManager.Instance.PushSubState(_mainMenuState);
        }

        public override void OnExit()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.ReleaseGamepad(_gamepadId);
                _gamepadId = 0;
                _gamepad = null;
            }

            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(_viewer);
            }
            _viewer = null;

            base.OnExit();
        }

        private void InitializeManagers()
        {
            ViewerManager.Instance.FreeViewers();
            ViewerManager.Instance.AllocateViewers(1);
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
