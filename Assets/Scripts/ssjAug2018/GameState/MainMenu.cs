using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class MainMenu : pdxpartyparrot.Game.State.SubGameState
    {
        [SerializeField]
        private Menu.MainMenu _menuPrefab;

        private Menu.MainMenu _menu;

        [SerializeField]
        private NetworkConnect _networkConnectStatePrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            _menu = Instantiate(_menuPrefab, UIManager.Instance.UIContainer.transform);
            _menu.NewGameState = _networkConnectStatePrefab;
        }

        public override void OnExit()
        {
            Destroy(_menu.gameObject);
            _menu = null;

            base.OnExit();
        }
    }
}
