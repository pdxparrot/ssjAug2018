using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class MainMenu : Game.Menu.Menu
    {
        public SubGameState NewGameState { private get; set; }

#region Event Handlers
        public void OnNewGame()
        {
            GameStateManager.Instance.PushSubState(NewGameState);
        }

        public void OnExit()
        {
            Application.Quit();
        }
#endregion
    }
}
