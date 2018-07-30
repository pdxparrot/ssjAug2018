using System;

using JetBrains.Annotations;

using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.Loading;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class GameStateManager : pdxpartyparrot.Game.State.GameStateManager<GameStateManager>
    {
        [SerializeField]
        private PlayerManager _playerManagerPrefab;

        private PlayerManager _playerManager;

        [SerializeField]
        private GameManager _gameManagerPrefab;

        private GameManager _gameManager;

        [CanBeNull]
        public NetworkClient NetworkClient { get; set; }

#region Unity Lifecycle
        private void Awake()
        {
            Core.Network.NetworkManager.Instance.ServerStartEvent += ServerStartEventHandler;
            Core.Network.NetworkManager.Instance.ServerStopEvent += ServerStopEventHandler;
        }

        protected override void OnDestroy()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerStopEvent -= ServerStopEventHandler;
                Core.Network.NetworkManager.Instance.ServerStartEvent -= ServerStartEventHandler;
            }

            base.OnDestroy();
        }
#endregion

        protected override void ShowLoadingScreen(bool show)
        {
            LoadingManager.Instance.ShowLoadingScreen(show);
        }

        protected  override void UpdateLoadingScreen(float percent, string text)
        {
            LoadingManager.Instance.UpdateLoadingScreen(percent, text);
        }

#region Event Handlers
        private void ServerStartEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Creating network managers");

            // NOTE: these manager prefabs must already be registered on the NetworkManager prefab for this to work
            // we don't need to call NetworkServer.Spawn here (I think) because the server will spawn them
            // for clients when they connect

            _gameManager = Instantiate(_gameManagerPrefab, transform);
            _playerManager = Instantiate(_playerManagerPrefab, transform);
        }

        private void ServerStopEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Destroying network managers");

            NetworkServer.Destroy(_playerManager.gameObject);
            _playerManager = null;

            NetworkServer.Destroy(_gameManager.gameObject);
            _gameManager = null;
        }
#endregion
    }
}
