using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.Actors;
using pdxpartyparrot.ssjAug2018.Items;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public abstract class BaseGame : pdxpartyparrot.Game.State.GameState
    {
        private ServerSpectator _serverSpectator;

        public override void OnEnter()
        {
            base.OnEnter();

            Initialize();

            Core.Network.NetworkManager.Instance.ServerDisconnectEvent += ServerDisconnectEventHandler;
            Core.Network.NetworkManager.Instance.ClientDisconnectEvent += ClientDisconnectEventHandler;
        }

        public override void OnExit()
        {
            if(null != _serverSpectator) {
                Destroy(_serverSpectator);
            }
            _serverSpectator = null;

            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerDisconnectEvent -= ServerDisconnectEventHandler;
                Core.Network.NetworkManager.Instance.ClientDisconnectEvent -= ClientDisconnectEventHandler;
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

        private void Initialize()
        {
            PartyParrotManager.Instance.IsPaused = false;

            DebugMenuManager.Instance.ResetFrameStats();

            InitializeServer();
            InitializeClient();

            ItemManager.Instance.PopulateItemPools();
        }

        private void InitializeServer()
        {
            if(!NetworkServer.active) {
                return;
            }

            Core.Network.NetworkManager.Instance.ServerChangedScene();

            if(!NetworkClient.active && !PartyParrotManager.Instance.IsHeadless) {
                ViewerManager.Instance.AllocateViewers(1, PlayerManager.Instance.PlayerData.ServerSpectatorViewer);

                InputManager.Instance.Controls.game.Enable();

                _serverSpectator = Instantiate(GameStateManager.Instance.GameData.ServerSpectatorPrefab);
            }

            GameManager.Instance.StartGame();
        }

        private void InitializeClient()
        {
            if(!NetworkClient.active) {
                return;
            }

            ViewerManager.Instance.AllocateViewers(1, PlayerManager.Instance.PlayerData.PlayerViewerPrefab);

            InputManager.Instance.Controls.game.Enable();

            Core.Network.NetworkManager.Instance.LocalClientReady(GameStateManager.Instance.NetworkClient?.connection, 0);
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
