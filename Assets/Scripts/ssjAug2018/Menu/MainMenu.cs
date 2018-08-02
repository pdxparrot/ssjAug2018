using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.GameState;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class MainMenu : Game.Menu.Menu
    {
        public SubGameState NewGameState { private get; set; }

        public SubGameState CreditsGameState { private get; set; }

#region Event Handlers
        public void OnNewGame()
        {
            GameStateManager.Instance.PushSubState(NewGameState);
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
