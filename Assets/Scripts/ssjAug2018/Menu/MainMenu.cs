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

        public NetworkConnect ConnectGameState { private get; set; }

        public Credits CreditsGameState { private get; set; }

#region Unity Lifecycle
        private void Awake()
        {
            if(!Application.isEditor) {
                _multiplayerButton.gameObject.SetActive(false);
            }

            _multiplayerPanel.gameObject.SetActive(false);
        }
#endregion

#region Event Handlers
        public void OnSinglePlayer()
        {
            GameStateManager.Instance.PushSubState(ConnectGameState, state => {
                state.Initialize(NetworkConnect.ConnectType.SinglePlayer);
            });
        }

        public void OnMultiplayer()
        {
            Owner.PushPanel(_multiplayerPanel);
        }

        public void OnCredits()
        {
            GameStateManager.Instance.PushSubState(CreditsGameState);
        }

        public void OnExit()
        {
            Application.Quit();
        }
#endregion
    }
}
