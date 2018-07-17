using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class MainMenu : Game.Menu.Menu
    {
        [SerializeField]
        private Game.State.GameState _gameStatePrefab;

#region Event Handlers
        public void OnNewGame()
        {
            GameStateManager.Instance.TransitionState(_gameStatePrefab);
        }

        public void OnExit()
        {
            Application.Quit();
        }
#endregion
    }
}
