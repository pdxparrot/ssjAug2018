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
         public new static PlayerManager Instance => (PlayerManager)NetworkActorManager.Instance;

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
        protected override void Awake()
        {
            base.Awake();

            _playerContainer = new GameObject("Players");
        }

        protected override void OnDestroy()
        {
            Destroy(_playerContainer);
            _playerContainer = null;

            base.OnDestroy();
        }
#endregion

        public void Initialize()
        {
            if(isServer) {
                Debug.Log("Registering player spawn function");
                Core.Network.NetworkManager.Instance.SetPlayerSpawnFunc(PlayerSpawnFunc);
            } else if(isClient) {
                Debug.Log("Registering spawnables");
            }
        }

        [CanBeNull]
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
