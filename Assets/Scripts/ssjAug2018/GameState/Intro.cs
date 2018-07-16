using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Intro : Game.State.GameState
    {
        [SerializeField]
        private MainMenu _mainMenuState;

        private Viewer _viewer;

        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();

            _viewer = ViewerManager.Instance.AcquireViewer();

            // TODO: acquire a gamepad
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            GameStateManager.Instance.PushSubState(_mainMenuState);
        }

        public override void OnExit()
        {
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
    }
}
