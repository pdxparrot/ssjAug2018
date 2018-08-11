using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ssjAug2018.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerManager : ActorManager<Player, PlayerManager>
    {
#region Data
        [SerializeField]
        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
#endregion

        [Space(10)]

        [SerializeField]
        private Player _playerPrefab;

        private GameObject _playerContainer;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        private void Awake()
        {
            _playerContainer = new GameObject("Players");

            Core.Network.NetworkManager.Instance.RegisterPlayerPrefab(_playerPrefab.NetworkPlayer);

            Core.Network.NetworkManager.Instance.ServerAddPlayerEvent += ServerAddPlayerEventHandler;

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerAddPlayerEvent -= ServerAddPlayerEventHandler;

                Core.Network.NetworkManager.Instance.UnregisterPlayerPrefab();
            }

            Destroy(_playerContainer);
            _playerContainer = null;

            DestroyDebugMenu();

            base.OnDestroy();
        }
#endregion

        //[Server]
        private void SpawnPlayer(NetworkConnection conn, short controllerId)
        {
            Assert.IsTrue(NetworkServer.active);

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return;
            }

            NetworkPlayer player = Core.Network.NetworkManager.Instance.SpawnPlayer<NetworkPlayer>(controllerId, conn, _playerContainer.transform);
            if(null == player) {
                Debug.LogError("Failed to spawn player!");
                return;
            }

            spawnPoint.Spawn(player.Player);
        }

        //[Server]
        public void RespawnPlayer(Player player)
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.Log($"Respawning player {player.name}");

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return;
            }

            spawnPoint.ReSpawn(player);
        }

#region Event Handlers
        private void ServerAddPlayerEventHandler(object sender, ServerAddPlayerEventArgs args)
        {
            SpawnPlayer(args.NetworkConnection, args.PlayerControllerId);
        }
#endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ssjAug2018.PlayerManager");
            _debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Players", GUI.skin.box);
                    foreach(Player player in Actors) {
                        GUILayout.Label($"{player.name} {player.transform.position}");
                    }
                GUILayout.EndVertical();
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
