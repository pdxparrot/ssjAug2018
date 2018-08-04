using System;

using pdxpartyparrot.Core.DebugMenu;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    // https://bitbucket.org/Unity-Technologies/networking
    // https://docs.unity3d.com/Manual/UNetGameObjects.html
    [RequireComponent(typeof(NetworkManagerHUD))]
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

/*
        // TODO: implement the use of this
        [SerializeField]
        private int _maxNetworkPlayers = 16;

        public int MaxNetworkPlayers => _maxNetworkPlayers;
*/

        [SerializeField]
        private bool _enableCallbackLogging = true;

        public NetworkManagerHUD HUD => GetComponent<NetworkManagerHUD>();

#region Unity Lifecycle
        // TODO: whenever this becomes a thing...
/*
        protected override void Awake()
        {
            base.Awake();

            autoCreatePlayer = false;
            HUD.showGUI = false;

            InitDebugMenu();
        }
*/

        private void Start()
        {
            autoCreatePlayer = false;
            HUD.showGUI = false;

            InitDebugMenu();
        }
#endregion

        public NetworkClient StartLANHost()
        {
            Debug.Log($"Starting LAN host on {networkAddress}:{networkPort}...");

            return StartHost();
        }

        public void Stop()
        {
            Debug.Log("Stopping host...");

            // TODO: don't assume this
            StopHost();
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
        public override void OnStartClient(NetworkClient client)
        {
            CallbackLog($"OnStartClient({client})");

            base.OnStartClient(client);
        }

        public override void OnStopClient()
        {
            CallbackLog($"OnStopClient()");

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
                HUD.showGUI = GUILayout.Toggle(HUD.showGUI, "Show Network HUD");
                _enableCallbackLogging = GUILayout.Toggle(_enableCallbackLogging, "Callback Logging");
            };
        }
    }
}
