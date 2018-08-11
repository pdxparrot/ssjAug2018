using System;

using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class NetworkConnect : SubGameState
    {
        public enum ConnectType
        {
            SinglePlayer,
            Server,
            Client
        }

        [SerializeField]
        private NetworkConnectUI _networkConnectUIPrefab;

        private NetworkConnectUI _networkConnectUI;

        private pdxpartyparrot.Game.State.GameState _gameStatePrefab;

        private Action<pdxpartyparrot.Game.State.GameState> _gameStateInit;

        [SerializeField]
        [ReadOnly]
        private ConnectType _connectType = ConnectType.SinglePlayer;

        public void Initialize(ConnectType connectType, pdxpartyparrot.Game.State.GameState gameStatePrefab, Action<pdxpartyparrot.Game.State.GameState> gameStateInit=null)
        {
            _connectType = connectType;
            _gameStatePrefab = gameStatePrefab;
            _gameStateInit = gameStateInit;
        }

        public void Cancel()
        {
            Core.Network.NetworkManager.Instance.DiscoverStop();

            switch(_connectType)
            {
            case ConnectType.SinglePlayer:
                Core.Network.NetworkManager.Instance.StopHost();
                break;
            case ConnectType.Server:
                Core.Network.NetworkManager.Instance.StopServer();
                break;
            case ConnectType.Client:
                Core.Network.NetworkManager.Instance.StopClient();
                break;
            }

            GameStateManager.Instance.TransitionToInitialState();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _networkConnectUI = Instantiate(_networkConnectUIPrefab, UIManager.Instance.UIContainer.transform);
            _networkConnectUI.Initialize(this);

            switch(_connectType)
            {
            case ConnectType.SinglePlayer:
                StartSinglePlayer();
                break;
            case ConnectType.Server:
                StartServer();
                break;
            case ConnectType.Client:
                StartClient();
                break;
            }
        }

        public override void OnExit()
        {
            if(null != _networkConnectUI) {
                Destroy(_networkConnectUI.gameObject);
                _networkConnectUI = null;
            }

            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.Discovery.ReceivedBroadcastEvent -= ReceivedBroadcastEventHandler;
                Core.Network.NetworkManager.Instance.DiscoverStop();

                Core.Network.NetworkManager.Instance.ServerConnectEvent -= ServerConnectEventHandler;
                Core.Network.NetworkManager.Instance.ClientConnectEvent -= ClientConnectEventHandler;
                Core.Network.NetworkManager.Instance.ClientSceneChangedEvent -= ClientSceneChangedEventHandler;
            }

            base.OnExit();
        }

        private void StartSinglePlayer()
        {
            Core.Network.NetworkManager.Instance.ServerConnectEvent += ServerConnectEventHandler;

            GameStateManager.Instance.NetworkClient = Core.Network.NetworkManager.Instance.StartHost();
            if(!NetworkClient.active) {
                _networkConnectUI.SetStatus("Unable to start network host!");
            }
        }

        private void StartServer()
        {
            Core.Network.NetworkManager.Instance.ServerConnectEvent += ServerConnectEventHandler;

            if(!Core.Network.NetworkManager.Instance.StartServer()) {
                _networkConnectUI.SetStatus("Unable to start network server!");
                return;
            }

            if(!Core.Network.NetworkManager.Instance.DiscoverServer()) {
                _networkConnectUI.SetStatus("Unable to start network server discovery!");
                return;
            }

            _networkConnectUI.SetStatus("Waiting for players...");
        }

        private void StartClient()
        {
            Core.Network.NetworkManager.Instance.Discovery.ReceivedBroadcastEvent += ReceivedBroadcastEventHandler;
            if(!Core.Network.NetworkManager.Instance.DiscoverClient()) {
                _networkConnectUI.SetStatus("Unable to start network client discovery!");
                return;
            }

            _networkConnectUI.SetStatus("Searching for server...");
        }

#region Event Handlers
        private void ServerConnectEventHandler(object sender, EventArgs args)
        {
            Core.Network.NetworkManager.Instance.DiscoverStop();

            _networkConnectUI.SetStatus("Client connected, loading scene...");

            Core.Network.NetworkManager.Instance.ServerChangeScene(_gameStatePrefab.SceneName);
            GameStateManager.Instance.TransitionState(_gameStatePrefab, _gameStateInit);
        }

        private void ClientConnectEventHandler(object sender, EventArgs args)
        {
            Core.Network.NetworkManager.Instance.DiscoverStop();

            _networkConnectUI.SetStatus("Connected, waiting for server...");

            Core.Network.NetworkManager.Instance.ClientSceneChangedEvent += ClientSceneChangedEventHandler;
        }

        private void ClientSceneChangedEventHandler(object sender, ClientSceneEventArgs args)
        {
            _networkConnectUI.SetStatus("Server ready, loading scene...");

            GameStateManager.Instance.TransitionState(_gameStatePrefab, _gameStateInit);
        }

        private void ReceivedBroadcastEventHandler(object sender, ReceivedBroadcastEventArgs args)
        {
            Core.Network.NetworkManager.Instance.Discovery.ReceivedBroadcastEvent -= ReceivedBroadcastEventHandler;
            Core.Network.NetworkManager.Instance.DiscoverStop();

            _networkConnectUI.SetStatus($"Found server at {args.EndPoint}, connecting...");

            Core.Network.NetworkManager.Instance.networkAddress = args.EndPoint.Address.ToString();
            Core.Network.NetworkManager.Instance.networkPort = args.EndPoint.Port;

            Core.Network.NetworkManager.Instance.ClientConnectEvent += ClientConnectEventHandler;

            GameStateManager.Instance.NetworkClient = Core.Network.NetworkManager.Instance.StartClient();
            if(null == GameStateManager.Instance.NetworkClient) {
                _networkConnectUI.SetStatus("Unable to start network client!");
                return;
            }
        }
#endregion
    }
}
