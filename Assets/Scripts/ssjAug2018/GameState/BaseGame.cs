using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public abstract class BaseGame : pdxpartyparrot.Game.State.GameState
    {
        [SerializeField]
        private Viewer _viewerPrefab;

        protected Viewer ViewerPrefab => _viewerPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();

            PartyParrotManager.Instance.IsPaused = false;

            DebugMenuManager.Instance.ResetFrameStats();

            NetworkManager.Instance.ServerDisconnectEvent += ServerDisconnectEventHandler;
            NetworkManager.Instance.ClientDisconnectEvent += ClientDisconnectEventHandler;
        }

        public override void OnExit()
        {
            if(NetworkManager.HasInstance) {
                NetworkManager.Instance.ServerDisconnectEvent -= ServerDisconnectEventHandler;
                NetworkManager.Instance.ClientDisconnectEvent -= ClientDisconnectEventHandler;
            }

            if(ItemManager.HasInstance) {
                ItemManager.Instance.FreeItemPools();
            }

            if(UIManager.HasInstance) {
                UIManager.Instance.Shutdown();
            }

            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.Disable();
            }

            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.ShutdownNetwork();
            }

            base.OnExit();
        }

        private void InitializeManagers()
        {
            ViewerManager.Instance.AllocateViewers(1, _viewerPrefab);

            InputManager.Instance.Controls.game.Enable();

            NetworkManager.Instance.LocalClientReady(GameStateManager.Instance.NetworkClient?.connection, 0);

            // TODO: this probably should wait until all of the clients are ready
            // is there a callback tho on the client that we can use as a "stop showing the loading screen" thing?
            NetworkManager.Instance.ServerChangedScene();

            GameManager.Instance.StartGame();

            ItemManager.Instance.PopulateItemPools();
        }

#region Event Handlers
        private void ServerDisconnectEventHandler(object sender, EventArgs args)
        {
            Debug.LogError("TODO: server disconnect");
        }

        private void ClientDisconnectEventHandler(object sender, EventArgs args)
        {
            Debug.LogError("TODO: client disconnect");
        }
#endregion
    }
}
