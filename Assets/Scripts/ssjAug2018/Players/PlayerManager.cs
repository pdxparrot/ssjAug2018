using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.World;
using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players
{
    public sealed class PlayerManager : ActorManager<Player>
    {
        [SerializeField]
        private Player _playerPrefab;

        private GameObject _playerContainer;

#region Unity Lifecycle
        private void Awake()
        {
            _playerContainer = new GameObject("Players");

            NetworkManager.Instance.SetPlayerSpawnFunc(PlayerSpawnFunc);
        }

        protected override void OnDestroy()
        {
            Destroy(_playerContainer);
            _playerContainer = null;

            base.OnDestroy();
        }
#endregion

        [CanBeNull]
        private NetworkActor PlayerSpawnFunc()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            if(null == spawnPoint) {
                return null;
            }

            Player player = Instantiate(_playerPrefab, _playerContainer.transform);
            if(!player.Initialize()) {
                return null;
            }

            spawnPoint.Spawn(player);
            return player;
        }
    }
}
