using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.Loading;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class GameStateManager : GameStateManager<GameStateManager>
    {
        [SerializeField]
        private GameData _gameData;

        public GameData GameData => _gameData;

        [SerializeField]
        private GameManager _gameManagerPrefab;

        private GameManager _gameManager;

        [SerializeField]
        private PlayerManager _playerManagerPrefab;

        private PlayerManager _playerManager;

        [SerializeField]
        private MailboxManager _mailboxManagerPrefab;

        private MailboxManager _mailboxManager;

        [SerializeField]
        private SceneTester _sceneTester;

        [CanBeNull]
        public NetworkClient NetworkClient { get; set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Core.Network.NetworkManager.Instance.ServerStartEvent += ServerStartEventHandler;
            Core.Network.NetworkManager.Instance.ServerStopEvent += ServerStopEventHandler;

            InitDebugMenu();
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

        protected override void UpdateLoadingScreen(float percent, string text)
        {
            LoadingManager.Instance.UpdateLoadingScreen(percent, text);
        }

        public void ShutdownNetwork()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.Stop();
            }

            NetworkClient = null;
        }

        //[Server]
        private void CreateNetworkManagers()
        {
            Debug.Log("Creating network managers");

// TODO: make sure these don't already exist

            // NOTE: these manager prefabs must already be registered on the NetworkManager prefab for this to work
            // we don't need to call NetworkServer.Spawn here (I think) because the server will spawn them
            // for clients when they connect

            _gameManager = Instantiate(_gameManagerPrefab, transform);
            _playerManager = Instantiate(_playerManagerPrefab, transform);
            _mailboxManager = Instantiate(_mailboxManagerPrefab, transform);
        }

        //[Server]
        private void DestroyNetworkManagers()
        {
            Debug.Log("Destroying network managers");

            NetworkServer.Destroy(_mailboxManager.gameObject);
            _mailboxManager = null;

            NetworkServer.Destroy(_playerManager.gameObject);
            _playerManager = null;

            NetworkServer.Destroy(_gameManager.gameObject);
            _gameManager = null;
        }

#region Event Handlers
        private void ServerStartEventHandler(object sender, EventArgs args)
        {
            CreateNetworkManagers();
        }

        private void ServerStopEventHandler(object sender, EventArgs args)
        {
            DestroyNetworkManagers();
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ssjAug2018.GameStateManager");
            debugMenuNode.RenderContentsAction = () => {
                foreach(string sceneName in _sceneTester.TestScenes) {
                    if(GUILayout.Button($"Load Test Scene {sceneName}")) {
                        TransitionState(_sceneTester, state => {
                            state.SetScene(sceneName);
                        });
                        break;
                    }
                }
            };
        }
    }
}
