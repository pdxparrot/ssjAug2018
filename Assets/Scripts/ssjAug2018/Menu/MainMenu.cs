using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.GameState;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class MainMenu : MenuPanel
    {
        [SerializeField]
        private Button _multiplayerButton;

        [SerializeField]
        private MultiplayerMenu _multiplayerPanel;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        private void Awake()
        {
            if(!Application.isEditor) {
                _multiplayerButton.gameObject.SetActive(false);
            }

            _multiplayerPanel.gameObject.SetActive(false);

            InitDebugMenu();
        }

        private void OnDestroy()
        {
            DestroyDebugMenu();
        }
#endregion

#region Event Handlers
        public void OnSinglePlayer()
        {
            GameStateManager.Instance.StartSinglePlayer();
        }

        public void OnMultiplayer()
        {
            Owner.PushPanel(_multiplayerPanel);
        }

        public void OnCredits()
        {
            GameStateManager.Instance.PushSubState(GameStateManager.Instance.CreditsStatePrefab);
        }

        public void OnQuitGame()
        {
            Application.Quit();
        }
#endregion

        private void InitDebugMenu()
        {
// TODO: this should change depending on if we're hosting/joining or whatever
// so that we don't get into a fucked up state
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Multiplayer Menu");
            _debugMenuNode.RenderContentsAction = () => {
                if(GUIUtils.LayoutButton("Host")) {
                    GameStateManager.Instance.StartHost();
                    return;
                }

                if(GUIUtils.LayoutButton("Join")) {
                    GameStateManager.Instance.StartJoin();
                    return;
                }
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
