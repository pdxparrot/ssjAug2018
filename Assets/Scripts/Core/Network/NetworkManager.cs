using System;

using pdxpartyparrot.Core.Actors;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    // https://bitbucket.org/Unity-Technologies/networking
    [RequireComponent(typeof(NetworkManagerHUD))]
    public sealed class NetworkManager : UnityEngine.Networking.NetworkManager
    {
        public static NetworkManager Instance => (NetworkManager)singleton;

        public static bool HasInstance => null != Instance;

        [SerializeField]
        private int _maxNetworkPlayers = 16;

        public int MaxNetworkPlayers => _maxNetworkPlayers;

        [SerializeField]
        private bool _enableCallbackLogging = true;

        public NetworkManagerHUD HUD => GetComponent<NetworkManagerHUD>();

        private Func<NetworkActor> _playerSpawnFunc;

        // TODO: whenever this becomes a thing...
/*
        protected override void Awake()
        {
            base.Awake();

            HUD.showGUI = false;
        }
*/

        private void Start()
        {
            autoCreatePlayer = true;
            HUD.showGUI = false;
        }

        public void StartLANHost()
        {
            Debug.Log($"Starting LAN host on {networkAddress}:{networkPort}...");

            StartHost();
        }

        public void Stop()
        {
            // TODO: don't assume this
            StopHost();
        }

        public void SetPlayerSpawnFunc(Func<NetworkActor> playerSpawnFunc)
        {
            _playerSpawnFunc = playerSpawnFunc;
        }

#region Server Callbacks
        public override void OnServerConnect(NetworkConnection conn)
        {
            CallbackLog($"OnServerConnect({conn})");

            base.OnServerConnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            CallbackLog($"OnServerAddPlayer({conn}, {playerControllerId})");

            NetworkActor player = _playerSpawnFunc?.Invoke();
            if(null == player) {
                Debug.LogError("Failed to spawn player!");
                return;
            }

            NetworkServer.AddPlayerForConnection(conn, player.gameObject, playerControllerId);

            player.OnSpawn();
        }

// TODO: more callbacks
#endregion

#region Client Callbacks
        public override void OnClientConnect(NetworkConnection conn)
        {
            CallbackLog($"OnClientConnect({conn})");

            base.OnClientConnect(conn);
        }

// TODO: more callbacks
#endregion

        private void CallbackLog(string message)
        {
            if(!_enableCallbackLogging) {
                return;
            }
            Debug.Log($"[NetworkManager]: {message}");
        }
    }
}
