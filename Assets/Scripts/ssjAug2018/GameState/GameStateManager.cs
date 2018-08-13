using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.Loading;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class GameStateManager : GameStateManager<GameStateManager>
    {
        [SerializeField]
        private GameData _gameData;

        public GameData GameData => _gameData;

        [Space(10)]

#region Game States
        [Header("Game States")]

        [SerializeField]
        private NetworkConnect _networkConnectStatePrefab;

        [SerializeField]
        private Credits _creditsStatePrefab;

        public Credits CreditsStatePrefab => _creditsStatePrefab;

        [SerializeField]
        private Game _gameStatePrefab;

        [SerializeField]
        private SceneTester _sceneTesterStatePrefab;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitDebugMenu();
        }
#endregion

        [CanBeNull]
        public NetworkClient NetworkClient { get; set; }

        protected override void ShowLoadingScreen(bool show)
        {
            LoadingManager.Instance.ShowLoadingScreen(show);
        }

        protected override void UpdateLoadingScreen(float percent, string text)
        {
            LoadingManager.Instance.UpdateLoadingScreen(percent, text);
        }

        public void StartSinglePlayer()
        {
            PushSubState(_networkConnectStatePrefab, state => {
                state.Initialize(NetworkConnect.ConnectType.SinglePlayer, _gameStatePrefab);
            });
        }

        public void StartHost()
        {
            PushSubState(_networkConnectStatePrefab, state => {
                state.Initialize(NetworkConnect.ConnectType.Server, _gameStatePrefab);
            });
        }

        public void StartJoin()
        {
            PushSubState(_networkConnectStatePrefab, state => {
                state.Initialize(NetworkConnect.ConnectType.Client, _gameStatePrefab);
            });
        }

        public void ShutdownNetwork()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.Stop();
            }
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ssjAug2018.GameStateManager");
            debugMenuNode.RenderContentsAction = () => {
                foreach(string sceneName in _sceneTesterStatePrefab.TestScenes) {
                    if(GUIUtils.LayoutButton($"Load Test Scene {sceneName}")) {
                        PushSubState(_networkConnectStatePrefab, connectState => {
                            connectState.Initialize(NetworkConnect.ConnectType.SinglePlayer, _sceneTesterStatePrefab, state => {
                                SceneTester sceneTester = (SceneTester)state;
                                sceneTester.SetScene(sceneName);
                            });
                        });
                        break;
                    }
                }
            };
        }
    }
}
