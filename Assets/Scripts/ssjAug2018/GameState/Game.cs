using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Game : pdxpartyparrot.Game.State.GameState
    {
        private Viewer _viewer;

        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();

            _viewer = ViewerManager.Instance.AcquireViewer();

            // TODO: acquire a gamepad

            NetworkManager.Instance.StartLANHost();
        }

        public override void OnExit()
        {
            NetworkManager.Instance.Stop();

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
