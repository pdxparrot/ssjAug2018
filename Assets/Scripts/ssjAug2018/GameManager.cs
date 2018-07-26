using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018
{
    public sealed class GameManager : SingletonBehavior<GameManager>
    {
#region Events
        public event EventHandler<EventArgs> GameReadyEvent;
#endregion

        [SerializeField]
        private PlayerManager _playerManagerPrefab;

        private PlayerManager _playerManager;

        [CanBeNull]
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

        public void Ready()
        {
            Core.Network.NetworkManager.Instance.ServerChangedScene();
            Core.Network.NetworkManager.Instance.LocalClientReady(NetworkClient?.connection, 0);

            MailboxManager.Instance.AllocateSize();
            MailboxManager.Instance.ActivateMailboxGroup(_playerManager.transform);
            
            GameReadyEvent?.Invoke(this, EventArgs.Empty);
        }

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
