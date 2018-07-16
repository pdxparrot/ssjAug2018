using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class MainMenu : Game.State.SubGameState
    {
        [SerializeField]
        private Game.Menu.Menu _menuPrefab;

        private Game.Menu.Menu _menu;

        public override void OnEnter()
        {
            base.OnEnter();

            _menu = Instantiate(_menuPrefab, transform);
        }

        public override void OnExit()
        {
            Destroy(_menu);
            _menu = null;

            base.OnExit();
        }
    }
}
