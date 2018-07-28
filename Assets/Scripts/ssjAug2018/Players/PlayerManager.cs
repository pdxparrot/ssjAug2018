using JetBrains.Annotations;

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
        }

        private void OnDestroy()
        {
            Destroy(_playerContainer);
            _playerContainer = null;

            if(isServer) {
                Debug.Log("Unregistering player spawn function");
                Core.Network.NetworkManager.Instance.SetPlayerSpawnFunc(null);
            }

            Instance = null;
        }
#endregion

        public void Initialize()
        {
            if(isServer) {
                Debug.Log("Registering player spawn function");
                Core.Network.NetworkManager.Instance.SetPlayerSpawnFunc(PlayerSpawnFunc);
            }
        }

        [CanBeNull]
        [Server]
        private NetworkActor PlayerSpawnFunc()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            if(null == spawnPoint) {
                return null;
            }

            Player player = Instantiate(_playerPrefab, _playerContainer.transform);
            spawnPoint.Spawn(player);
            return player;
        }
    }
}
