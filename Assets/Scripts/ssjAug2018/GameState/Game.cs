using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Game : pdxpartyparrot.Game.State.GameState
    {
        [SerializeField]
        private Viewer _viewerPrefab;

        [SerializeField]
        private GameOver _gameOverState;

        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();
        }

        public override void OnUpdate(float dt)
        {
            if(GameManager.Instance.IsGameOver) {
                GameStateManager.Instance.PushSubState(_gameOverState);
            }
        }

        public override void OnExit()
        {
            if(UIManager.HasInstance) {
                UIManager.Instance.Shutdown();
            }

            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.Disable();
            }

            if(NetworkManager.HasInstance) {
                NetworkManager.Instance.Stop();
            }

            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.NetworkClient = null;
            }

            base.OnExit();
        }

        private void InitializeManagers()
        {
            ViewerManager.Instance.AllocateViewers(1, _viewerPrefab);

            InputManager.Instance.Controls.game.Enable();

            NetworkManager.Instance.LocalClientReady(GameStateManager.Instance.NetworkClient?.connection, 0);

            // TODO: this probably should wait until all of the clients are ready
            // is there a callback tho on the client that we can use as a "stop showing the loading screen" thing?
            NetworkManager.Instance.ServerChangedScene();
            GameManager.Instance.StartGame();
        }
    }
}
