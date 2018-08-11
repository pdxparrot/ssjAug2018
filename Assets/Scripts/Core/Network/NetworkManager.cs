using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace pdxpartyparrot.Core.Network
{
    // https://bitbucket.org/Unity-Technologies/networking
    [RequireComponent(typeof(NetworkManagerHUD))]
    [RequireComponent(typeof(NetworkDiscovery))]
    public sealed class NetworkManager : UnityEngine.Networking.NetworkManager
    {
#region Events
        public event EventHandler<EventArgs> ServerStartEvent;
        public event EventHandler<EventArgs> ServerStopEvent;
        public event EventHandler<EventArgs> ServerConnectEvent;
        public event EventHandler<EventArgs> ServerDisconnectEvent;
        public event EventHandler<EventArgs> ServerChangeSceneEvent;
        public event EventHandler<EventArgs> ServerChangedSceneEvent;
        public event EventHandler<ServerAddPlayerEventArgs> ServerAddPlayerEvent;

        public event EventHandler<EventArgs> ClientConnectEvent;
        public event EventHandler<EventArgs> ClientDisconnectEvent;
        public event EventHandler<ClientSceneEventArgs> ClientSceneChangeEvent;
        public event EventHandler<ClientSceneEventArgs> ClientSceneChangedEvent;
#endregion

#region Messages
        public class CustomMsgType
        {
            public const short SceneChange  = MsgType.Highest + 1;
            public const short SceneChanged = MsgType.Highest + 2;

            // NOTE: always last, always highest
            public const short Highest = MsgType.Highest + 3;
        }
#endregion

#region Singleton
        public static NetworkManager Instance => (NetworkManager)singleton;

        public static bool HasInstance => null != Instance;
#endregion

        [SerializeField]
        private bool _enableCallbackLogging = true;

        private NetworkManagerHUD _hud;

        public NetworkDiscovery Discovery { get; private set; }

#region Unity Lifecycle
        // TODO: whenever this becomes a thing...
/*
        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }
*/

        private void Start()
        {
            Initialize();
        }
#endregion

        private void Initialize()
        {
            _hud = GetComponent<NetworkManagerHUD>();
            _hud.showGUI = false;

            Discovery = GetComponent<NetworkDiscovery>();
            Discovery.useNetworkManager = true;
            Discovery.showGUI = false;
            Discovery.enabled = PartyParrotManager.Instance.Config.Network.Discovery.Enable;

            autoCreatePlayer = false;

            InitDebugMenu();
        }

#region Network Prefabs
        public void RegisterNetworkPrefab<T>(T networkPrefab) where T: NetworkBehaviour
        {
            Debug.Log($"[NetworkManager]: Registering network prefab {networkPrefab.name}");
            ClientScene.RegisterPrefab(networkPrefab.gameObject);
        }

        public void UnregisterNetworkPrefab<T>(T networkPrefab) where T: NetworkBehaviour
        {
            Debug.Log($"[NetworkManager]: Unregistering network prefab {networkPrefab.name}");
            ClientScene.UnregisterPrefab(networkPrefab.gameObject);
        }

        [CanBeNull]
        public T SpawnNetworkPrefab<T>(T networkPrefab) where T: NetworkBehaviour
        {
            if(!NetworkServer.active) {
                Debug.LogWarning("Cannot spawn network prefab without an active server!");
                return null;
            }

            Debug.Log($"[NetworkManager]: Spawning network prefab {networkPrefab.name}");

            T obj = Instantiate(networkPrefab);
            if(null == obj) {
                return null;
            }

            SpawnNetworkObject(obj);
            return obj;
        }

        [CanBeNull]
        public T SpawnNetworkPrefab<T>(T networkPrefab, Transform parent) where T: NetworkBehaviour
        {
            T obj = SpawnNetworkPrefab(networkPrefab);
            if(null == obj) {
                return null;
            }
            obj.transform.SetParent(parent, true);
            return obj;
        }

        public void SpawnNetworkObject<T>([NotNull] T networkObject) where T: NetworkBehaviour
        {
            NetworkServer.Spawn(networkObject.gameObject);
        }

        public void UnSpawnNetworkObject<T>([NotNull] T networkObject) where T: NetworkBehaviour
        {
            NetworkServer.UnSpawn(networkObject.gameObject);
        }

        public void DestroyNetworkObject<T>([CanBeNull] T networkObject) where T: NetworkBehaviour
        {
            if(null == networkObject) {
                return;
            }

            if(!NetworkServer.active) {
                Debug.LogWarning("Cannot destroy network object without an active server!");
                return;
            }

            Debug.Log($"[NetworkManager]: Destroying network object {networkObject.name}");

            NetworkServer.Destroy(networkObject.gameObject);
        }
#endregion

#region Player Prefab
        public void RegisterPlayerPrefab<T>(T prefab) where T: NetworkActor
        {
            Debug.Log($"[NetworkManager]: Registering player prefab {prefab.name}");
            // TODO: warn if already set?
            playerPrefab = prefab.gameObject;
            RegisterNetworkPrefab(prefab);
        }

        public void UnregisterPlayerPrefab()
        {
            Debug.Log($"[NetworkManager]: Unregistering player prefab {playerPrefab.name}");
            // TODO: warn if not set?
            UnregisterNetworkPrefab(playerPrefab.GetComponent<NetworkBehaviour>());
            playerPrefab = null;
        }

        public T SpawnPlayer<T>(short controllerId, NetworkConnection conn) where T: NetworkActor
        {
            if(!NetworkServer.active) {
                Debug.LogWarning("Cannot spawn player prefab without an active server!");
                return null;
            }

            if(null == playerPrefab) {
                Debug.LogWarning("Player prefab not registered!");
                return null;
            }

            GameObject player = Instantiate(playerPrefab);
            if(null == player) {
                Debug.LogError("Failed to spawn player!");
                return null;
            }
            player.name = $"Player {controllerId}";

            // call this instead of NetworkServer.Spawn()
            NetworkServer.AddPlayerForConnection(conn, player.gameObject, controllerId);
            return player.GetComponent<T>();
        }

        public T SpawnPlayer<T>(short controllerId, NetworkConnection conn, Transform parent) where T: NetworkActor
        {
            T player = SpawnPlayer<T>(controllerId, conn);
            if(null == player) {
                return null;
            }
            player.transform.SetParent(parent, true);
            return player;
        }

        public void DespawnPlayers(NetworkConnection conn)
        {
            if(!NetworkServer.active) {
                Debug.LogWarning("Cannot despawn players without an active server!");
                return;
            }

            NetworkServer.DestroyPlayersForConnection(conn);
        }
#endregion

#region Discovery
        private bool InitDiscovery()
        {
            Discovery.broadcastPort = PartyParrotManager.Instance.Config.Network.Discovery.Port;
            return Discovery.Initialize();
        }

        public bool DiscoverServer()
        {
            if(!Discovery.enabled) {
                return true;
            }

            Debug.Log("[NetworkManager]: Starting server discovery");

            if(!InitDiscovery()) {
                return false;
            }

            return Discovery.StartAsServer();
        }

        public bool DiscoverClient()
        {
            if(!Discovery.enabled) {
                return true;
            }

            Debug.Log("[NetworkManager]: Starting client discovery");

            if(!InitDiscovery()) {
                return false;
            }

            return Discovery.StartAsClient();
        }

        public void DiscoverStop()
        {
            if(Discovery.running) {
                Debug.Log("[NetworkManager]: Stopping discovery");
                Discovery.StopBroadcast();
            }
        }
#endregion

        public override NetworkClient StartHost()
        {
            Debug.Log("[NetworkManager]: Starting LAN host");

            maxConnections = PartyParrotManager.Instance.Config.Network.Server.MaxConnections;
            NetworkClient networkClient = base.StartHost();
            if(null == networkClient) {
                return null;
            }

            InitClient(networkClient);
            return networkClient;
        }

        public new bool StartServer()
        {
            maxConnections = PartyParrotManager.Instance.Config.Network.Server.MaxConnections;
            networkAddress = PartyParrotManager.Instance.Config.Network.Server.NetworkAddress;
            networkPort = PartyParrotManager.Instance.Config.Network.Server.Port;

            if(PartyParrotManager.Instance.Config.Network.Server.BindIp()) {
                serverBindAddress = PartyParrotManager.Instance.Config.Network.Server.NetworkAddress;
                serverBindToIP = true;

                Debug.Log($"[NetworkManager]: Binding to address {serverBindAddress}");
            }

            Debug.Log($"[NetworkManager]: Listening for clients on {networkAddress}:{networkPort}");
            return base.StartServer();
        }

        public new NetworkClient StartClient()
        {
            Debug.Log($"[NetworkManager]: Connecting client to {networkAddress}:{networkPort}");
            NetworkClient networkClient = base.StartClient();
            if(null == networkClient) {
                return null;
            }

            InitClient(networkClient);
            return networkClient;
        }

        private void InitClient(NetworkClient networkClient)
        {
            networkClient.RegisterHandler(CustomMsgType.SceneChange, OnClientCustomSceneChange);
            networkClient.RegisterHandler(CustomMsgType.SceneChanged, OnClientCustomSceneChanged);
        }

        public void Stop()
        {
            if(NetworkServer.active && NetworkClient.active) {
                StopHost();
            } else if(NetworkServer.active) {
                StopServer();
            } else if(NetworkClient.active) {
                StopClient();
            }
        }

        public void LocalClientReady(NetworkConnection conn, short playerControllerId)
        {
            if(null == conn) {
                return;
            }

            Debug.Log("[NetworkManager]: Local client ready!");

            ClientScene.Ready(conn);
            ClientScene.AddPlayer(playerControllerId);
        }

        public override void ServerChangeScene(string sceneName)
        {
            Debug.Log($"[NetworkManager]: Server changing to scene {sceneName}...");

            NetworkServer.SetAllClientsNotReady();
            networkSceneName = sceneName;

            ServerChangeSceneEvent?.Invoke(this, EventArgs.Empty);

            StringMessage msg = new StringMessage(networkSceneName);
            NetworkServer.SendToAll(CustomMsgType.SceneChange, msg);
        }

        public void ServerChangedScene()
        {
            if(!NetworkServer.active) {
                return;
            }

            ServerChangedSceneEvent?.Invoke(this, EventArgs.Empty);

            NetworkServer.SpawnObjects();
            OnServerSceneChanged(networkSceneName);
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

            StringMessage msg = new StringMessage(networkSceneName);
            NetworkServer.SendToAll(CustomMsgType.SceneChanged, msg);
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

        public void OnClientCustomSceneChange(NetworkMessage netMsg)
        {
            CallbackLog($"OnClientCustomSceneChange({netMsg})");

            string sceneName = netMsg.reader.ReadString();
            ClientSceneChangeEvent?.Invoke(this, new ClientSceneEventArgs(sceneName));
        }

        public void OnClientCustomSceneChanged(NetworkMessage netMsg)
        {
            CallbackLog($"OnClientCustomSceneChanged({netMsg})");

            string sceneName = netMsg.reader.ReadString();
            ClientSceneChangedEvent?.Invoke(this, new ClientSceneEventArgs(sceneName));
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

                if(Discovery.enabled) {
                    Discovery.showGUI = GUILayout.Toggle(Discovery.showGUI, "Show Network Discovery GUI");
                }

                _enableCallbackLogging = GUILayout.Toggle(_enableCallbackLogging, "Callback Logging");
            };
        }
    }
}
