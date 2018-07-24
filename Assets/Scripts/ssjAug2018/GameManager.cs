using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018
{
    public sealed class GameManager : SingletonBehavior<GameManager>
    {
        [SerializeField]
        private PlayerManager _playerManagerPrefab;

        private PlayerManager _playerManager;

        public NetworkClient NetworkClient { get; set; }

#region Unity Lifecycle
        private void Awake()
        {
            Core.Network.NetworkManager.Instance.ServerConnectEvent += ServerConnectEventHandler;
            Core.Network.NetworkManager.Instance.ServerDisconnectEvent += ServerDisconnectEventHandler;
        }

        protected override void OnDestroy()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerConnectEvent -= ServerConnectEventHandler;
                Core.Network.NetworkManager.Instance.ServerDisconnectEvent -= ServerDisconnectEventHandler;
            }

            base.OnDestroy();
        }
#endregion

#region Event Handlers
        private void ServerConnectEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Creating network managers");

            // NOTE: these manager prefabs must already be registered on the NetworkManager prefab for this to work

            _playerManager = Instantiate(_playerManagerPrefab, transform);
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
