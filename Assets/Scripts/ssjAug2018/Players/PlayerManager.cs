using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ssjAug2018.Data;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerManager : NetworkActorManager
    {
        public static PlayerManager Instance { get; private set; }

        public static bool HasInstance => null != Instance;

#region Data
        [SerializeField]
        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
#endregion

        [Space(10)]

        [SerializeField]
        private Player _playerPrefab;

        private GameObject _playerContainer;

#region Unity Lifecycle
        private void Awake()
        {
            if(HasInstance) {
                Debug.LogError($"[NetworkSingleton] Instance already created: {Instance.gameObject.name}");
                return;
            }

            Instance = this;

            _playerContainer = new GameObject("Players");

            Core.Network.NetworkManager.Instance.ServerAddPlayerEvent += ServerAddPlayerEventHandler;
        }

        private void OnDestroy()
        {
            Destroy(_playerContainer);
            _playerContainer = null;

            Core.Network.NetworkManager.Instance.ServerAddPlayerEvent -= ServerAddPlayerEventHandler;

            Instance = null;
        }
#endregion

        [Server]
        private void SpawnPlayer(NetworkConnection conn, short controllerId)
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            if(null == spawnPoint) {
                return;
            }

            Player player = Instantiate(_playerPrefab, _playerContainer.transform);
            if(null == player) {
                Debug.LogError("Failed to spawn player!");
                return;
            }
            spawnPoint.Spawn(player);

            NetworkServer.AddPlayerForConnection(conn, player.gameObject, controllerId);

            player.OnSpawn();
        }

#region Event Handlers
        private void ServerAddPlayerEventHandler(object sender, ServerAddPlayerEventArgs args)
        {
            SpawnPlayer(args.NetworkConnection, args.PlayerControllerId);
        }
#endregion
    }
}
