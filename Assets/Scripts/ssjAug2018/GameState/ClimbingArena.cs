using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class ClimbingArena : pdxpartyparrot.Game.State.GameState
    {
        [SerializeField]
        private Viewer _viewerPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            if(null == GameStateManager.Instance.NetworkClient) {
                GameStateManager.Instance.NetworkClient = Core.Network.NetworkManager.Instance.StartLANHost();
                Core.Network.NetworkManager.Instance.ServerChangeScene();
            }

            InitializeManagers();
        }

        public override void OnExit()
        {
            if(ItemManager.HasInstance) {
                ItemManager.Instance.FreeItemPools();
            }

            if(UIManager.HasInstance) {
                UIManager.Instance.Shutdown();
            }

            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.Disable();
            }

            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.Stop();
            }

            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.NetworkClient = null;
            }

            base.OnExit();
        }

        private void InitializeManagers()
        {
            ViewerManager.Instance.AllocateViewers(1, _viewerPrefab);

            InputManager.Instance.Controls.game.Enable();

            Core.Network.NetworkManager.Instance.LocalClientReady(GameStateManager.Instance.NetworkClient?.connection, 0);

            Core.Network.NetworkManager.Instance.ServerChangedScene();

            ItemManager.Instance.PopulateItemPools();
        }
    }
}
