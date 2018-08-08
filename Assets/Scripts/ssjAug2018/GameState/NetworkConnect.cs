using System;

using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class NetworkConnect : SubGameState
    {
        public enum ConnectType
        {
            SinglePlayer,
            Host,
            Client
        }

        [SerializeField]
        private pdxpartyparrot.Game.State.GameState _gameStatePrefab;

        [SerializeField]
        [ReadOnly]
        private ConnectType _connectType = ConnectType.SinglePlayer;

        public void Initialize(ConnectType connectType)
        {
            _connectType = connectType;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            switch(_connectType)
            {
            case ConnectType.SinglePlayer:
                Core.Network.NetworkManager.Instance.ClientConnectEvent += ClientConnectEventHandler;
                GameStateManager.Instance.NetworkClient = Core.Network.NetworkManager.Instance.StartLANHost();
                break;
            case ConnectType.Host:
                // TODO: when do we transition??
                GameStateManager.Instance.NetworkClient = Core.Network.NetworkManager.Instance.StartHost();
                break;
            case ConnectType.Client:
                Debug.Log("TODO: show connecting screen");
                Core.Network.NetworkManager.Instance.ClientConnectEvent += ClientConnectEventHandler;
// TODO: handle connection errors
                GameStateManager.Instance.NetworkClient = Core.Network.NetworkManager.Instance.StartClient();
                break;
            }
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

// TODO: possible UI updates here
        }

        public override void OnExit()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ClientConnectEvent -= ClientConnectEventHandler;

                if(NetworkServer.active) {
                    Core.Network.NetworkManager.Instance.ServerChangeScene();
                }
            }

            base.OnExit();
        }

#region Event Handlers
        private void ClientConnectEventHandler(object sender, EventArgs args)
        {
            Debug.Log("TODO: hide connecting screen");

            GameStateManager.Instance.TransitionState(_gameStatePrefab);
        }
#endregion
    }
}
