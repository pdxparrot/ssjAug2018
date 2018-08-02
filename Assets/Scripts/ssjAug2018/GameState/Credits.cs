using pdxpartyparrot.ssjAug2018.UI;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Credits : pdxpartyparrot.Game.State.SubGameState
    {
        [SerializeField]
        private Menu.CreditsMenu _menuPrefab;

        private Menu.CreditsMenu _menu;

        public override void OnEnter()
        {
            base.OnEnter();

            _menu = Instantiate(_menuPrefab, UIManager.Instance.UIContainer.transform);
        }

        public override void OnExit()
        {
            Destroy(_menu.gameObject);
            _menu = null;

            base.OnExit();
        }
    }
}
