using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Intro : Game.State.GameState
    {
        private Viewer _viewer;

        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();

            _viewer = ViewerManager.Instance.AcquireViewer();

            // TODO: acquire a gamepad
        }

        public override void OnExit()
        {
            ViewerManager.Instance.ReleaseViewer(_viewer);
            _viewer = null;
        }

        private void InitializeManagers()
        {
            ViewerManager.Instance.FreeViewers();
            ViewerManager.Instance.AllocateViewers(1);
        }
    }
}
