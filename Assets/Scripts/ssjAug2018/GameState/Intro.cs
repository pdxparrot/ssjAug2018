using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Intro : Game.State.GameState
    {
        [SerializeField]
        private Game.Menu.Menu _menuPrefab;

        private Viewer _viewer;

        private Game.Menu.Menu _menu;

        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();

            _menu = Instantiate(_menuPrefab, transform);

            _viewer = ViewerManager.Instance.AcquireViewer();

            // TODO: acquire a gamepad
        }

        public override void OnExit()
        {
            ViewerManager.Instance.ReleaseViewer(_viewer);
            _viewer = null;

            Destroy(_menu);
            _menu = null;
        }

        private void InitializeManagers()
        {
            ViewerManager.Instance.FreeViewers();
            ViewerManager.Instance.AllocateViewers(1);
        }
    }
}
