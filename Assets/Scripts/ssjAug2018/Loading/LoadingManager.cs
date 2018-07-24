using System;

using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
#region Manager Prefabs
        [SerializeField]
        private UIManager _uiManagerPrefab;

        [SerializeField]
        private PlayerManager _playerManagerPrefab;

        private PlayerManager _playerManager;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            Core.Network.NetworkManager.Instance.ServerConnectEvent += ServerConnectEventHandler;
            Core.Network.NetworkManager.Instance.ServerDisconnectEvent += ServerDisconnectEventHandler;

            UIManager.CreateFromPrefab(_uiManagerPrefab, ManagersContainer);
            SpawnManager.Create(ManagersContainer);
        }

#region Event Handlers
        private void ServerConnectEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Spawning network managers");

            // NOTE: these manager prefabs must already be registered on the NetworkManager prefab for this to work

            _playerManager = Instantiate(_playerManagerPrefab, ManagersContainer.transform);
            NetworkServer.Spawn(_playerManager.gameObject);
            _playerManager.Initialize();
        }

        private void ServerDisconnectEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Destroying network managers");

            Destroy(_playerManager.gameObject);
            _playerManager = null;
        }
#endregion
    }
}
