using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.UI
{
    public sealed class UIManager : UIManager<UIManager>
    {
        [SerializeField]
        private PlayerUI _playerUIPrefab;

        private PlayerUI _playerUI;

        public PlayerUI PlayerUI => _playerUI;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            InitDebugMenu();
        }
#endregion

        public void InitializePlayerUI(Player player)
        {
            _playerUI = Instantiate(_playerUIPrefab, UIContainer.transform);
            _playerUI.Initialize(player);
        }

        public void Shutdown()
        {
            if(null != _playerUI) {
                Destroy(_playerUI.gameObject);
            }
            _playerUI = null;
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ssjAug2018.UIManager");
            debugMenuNode.RenderContentsAction = () => {
                if(GUILayout.Button("Toggle HUD")) {
                    PlayerUI.ToggleHUD();
                }
            };
        }
    }
}
