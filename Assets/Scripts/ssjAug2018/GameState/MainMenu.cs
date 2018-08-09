using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class MainMenu : pdxpartyparrot.Game.State.SubGameState
    {
        [SerializeField]
        private pdxpartyparrot.Game.Menu.Menu _menuPrefab;

        private pdxpartyparrot.Game.Menu.Menu _menu;

        [SerializeField]
        private Credits _creditsStatePrefab;

        [SerializeField]
        private Game _gameStatePrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            _menu = Instantiate(_menuPrefab, UIManager.Instance.UIContainer.transform);

            // TODO: this is shitty
            Menu.MainMenu mainMenu = _menu.GetComponentInChildren<Menu.MainMenu>(true);
            mainMenu.ConnectGameState = GameStateManager.Instance.NetworkConnectStatePrefab;
            mainMenu.CreditsGameState = _creditsStatePrefab;
            mainMenu.GameState = _gameStatePrefab;

            // TODO: this is also shitty
            Menu.MultiplayerMenu multiplayerMenu = _menu.GetComponentInChildren<Menu.MultiplayerMenu>(true);
            multiplayerMenu.ConnectGameState = GameStateManager.Instance.NetworkConnectStatePrefab;
            multiplayerMenu.GameState = _gameStatePrefab;
        }

        public override void OnExit()
        {
            Destroy(_menu.gameObject);
            _menu = null;

            base.OnExit();
        }

        public override void OnResume()
        {
            base.OnResume();

            _menu.gameObject.SetActive(true);
        }

        public override void OnPause()
        {
            _menu.gameObject.SetActive(false);

            base.OnPause();
        }
    }
}
