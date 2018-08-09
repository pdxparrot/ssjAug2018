using System;

using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.DebugMenu;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    // https://bitbucket.org/Unity-Technologies/networking
    // https://docs.unity3d.com/Manual/UNetGameObjects.html
    [RequireComponent(typeof(NetworkManagerHUD))]
    [RequireComponent(typeof(NetworkDiscovery))]
    public sealed class NetworkManager : UnityEngine.Networking.NetworkManager
    {
#region Events
        public event EventHandler<EventArgs> ServerStartEvent;
        public event EventHandler<EventArgs> ServerStopEvent;
        public event EventHandler<EventArgs> ServerConnectEvent;
        public event EventHandler<EventArgs> ServerDisconnectEvent;
        public event EventHandler<ServerAddPlayerEventArgs> ServerAddPlayerEvent;

        public event EventHandler<EventArgs> ClientConnectEvent;
        public event EventHandler<EventArgs> ClientDisconnectEvent;
#endregion

        public static NetworkManager Instance => (NetworkManager)singleton;

        public static bool HasInstance => null != Instance;

        [SerializeField]
        private NetworkManagerData _data;

        public NetworkManagerData Data => _data;

        [SerializeField]
        private bool _enableCallbackLogging = true;

        private NetworkManagerHUD _hud;

        private NetworkDiscovery _networkDiscovery;

#region Unity Lifecycle
        // TODO: whenever this becomes a thing...
/*
        protected override void Awake()
        {
            base.Awake();

            _hud = GetComponent<NetworkManagerHUD>();
            _hud.showGUI = false;

            _networkDiscovery = GetComponent<NetworkDiscovery>();
            _networkDiscovery.enabled = _enableDiscovery;
            _networkDiscovery.showGUI = false;

            autoCreatePlayer = false;

            InitDebugMenu();
        }
*/

        private void Start()
        {
            _hud = GetComponent<NetworkManagerHUD>();
            _hud.showGUI = false;

            _networkDiscovery = GetComponent<NetworkDiscovery>();
            _networkDiscovery.useNetworkManager = true;
            _networkDiscovery.showGUI = false;
            _networkDiscovery.enabled = Data.EnableDiscovery;

            autoCreatePlayer = false;

            InitDebugMenu();
        }
#endregion

#region Discovery
        public bool DiscoverServer()
        {
            if(!Data.EnableDiscovery) {
                return true;
            }

            Debug.Log("[NetworkManager]: Starting server discovery");

            if(!_networkDiscovery.Initialize()) {
                return false;
            }

            return _networkDiscovery.StartAsServer();
        }

        public bool DiscoverClient()
        {
            if(!Data.EnableDiscovery) {
                return true;
            }

            Debug.Log("[NetworkManager]: Starting client discovery");

            if(!_networkDiscovery.Initialize()) {
                return false;
            }

            return _networkDiscovery.StartAsClient();
        }

        public void DiscoverStop()
        {
            Debug.Log("[NetworkManager]: Stopping discovery");

            _networkDiscovery.StopBroadcast();
        }
#endregion

        public override NetworkClient StartHost()
        {
            maxConnections = Data.MaxNetworkPlayers;
            return base.StartHost();
        }

        public new bool StartServer()
        {
            maxConnections = Data.MaxNetworkPlayers;
            return base.StartServer();
        }

        public void LocalClientReady(NetworkConnection conn, short playerControllerId)
        {
            if(null == conn) {
                return;
            }

            ClientScene.Ready(conn);
            ClientScene.AddPlayer(playerControllerId);
        }

        public void ServerChangeScene()
        {
            Debug.Log("Server changing scene...");

            NetworkServer.SetAllClientsNotReady();
        }

        public void ServerChangedScene()
        {
            if(!NetworkServer.active) {
                return;
            }

            Debug.Log("Server changed scene...");

            NetworkServer.SpawnObjects();
        }

#region Server Callbacks
        public override void OnStartHost()
        {
            CallbackLog("OnStartHost");

            base.OnStartHost();
        }

        public override void OnStopHost()
        {
            CallbackLog("OnStopHost");

            base.OnStopHost();
        }

        public override void OnStartServer()
        {
            CallbackLog("OnStartServer");

            base.OnStartServer();

            ServerStartEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void OnStopServer()
        {
            CallbackLog("OnStopServer");

            ServerStopEvent?.Invoke(this, EventArgs.Empty);

            base.OnStopServer();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            CallbackLog($"OnServerConnect({conn})");

            base.OnServerConnect(conn);

            ServerConnectEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            CallbackLog($"OnServerDisconnect({conn})");

            ServerDisconnectEvent?.Invoke(this, EventArgs.Empty);

            base.OnServerDisconnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            CallbackLog($"OnServerAddPlayer({conn}, {playerControllerId})");

            ServerAddPlayerEvent?.Invoke(this, new ServerAddPlayerEventArgs(conn, playerControllerId));

            // NOTE: do not call the base method
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            CallbackLog($"OnServerRemovePlayer({conn}, {player})");

            base.OnServerRemovePlayer(conn, player);
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            CallbackLog($"OnServerReady({conn})");

            base.OnServerReady(conn);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            CallbackLog($"OnServerSceneChanged({sceneName})");

            base.OnServerSceneChanged(sceneName);
        }
#endregion

#region Client Callbacks
        public override void OnStartClient(NetworkClient networkClient)
        {
            CallbackLog($"OnStartClient({networkClient})");

            base.OnStartClient(networkClient);
        }

        public override void OnStopClient()
        {
            CallbackLog("OnStopClient()");

            base.OnStopClient();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            CallbackLog($"OnClientConnect({conn})");

            ClientConnectEvent?.Invoke(this, EventArgs.Empty);

            // NOTE: do not call the base method
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            CallbackLog($"OnClientDisconnect({conn})");

            base.OnClientDisconnect(conn);

            ClientDisconnectEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            CallbackLog($"OnClientSceneChanged({conn})");

            base.OnClientSceneChanged(conn);
        }
#endregion

        private void CallbackLog(string message)
        {
            if(!_enableCallbackLogging) {
                return;
            }
            Debug.Log($"[NetworkManager]: {message}");
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.NetworkManager");
            debugMenuNode.RenderContentsAction = () => {
                if(_hud.enabled) {
                    _hud.showGUI = GUILayout.Toggle(_hud.showGUI, "Show Network HUD GUI");
                }

                if(_networkDiscovery.enabled) {
                    _networkDiscovery.showGUI = GUILayout.Toggle(_networkDiscovery.showGUI, "Show Network Discovery GUI");
                }

                _enableCallbackLogging = GUILayout.Toggle(_enableCallbackLogging, "Callback Logging");
            };
        }
    }
}
