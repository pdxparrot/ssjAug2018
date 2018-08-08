using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.ssjAug2018.Menu;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.UI
{
    public sealed class UIManager : UIManager<UIManager>
    {
        [SerializeField]
        private PlayerUI _playerUIPrefab;

        [CanBeNull]
        private PlayerUI _playerUI;

        [CanBeNull]
        public PlayerUI PlayerUI => _playerUI;

        [SerializeField]
        private PauseMenu _pauseMenuPrefab;

        [CanBeNull]
        private PauseMenu _pauseMenu;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }
#endregion

        public void InitializePlayerUI(Player player)
        {
            _playerUI = Instantiate(_playerUIPrefab, UIContainer.transform);
            if(null != _playerUI) {
                _playerUI.Initialize(player);
            }

            _pauseMenu = Instantiate(_pauseMenuPrefab, UIContainer.transform);
            if(null != _pauseMenu) {
                _pauseMenu.gameObject.SetActive(PartyParrotManager.Instance.IsPaused);
            }
        }

        public void Shutdown()
        {
            if(null != _pauseMenu) {
                Destroy(_pauseMenu.gameObject);
            }
            _pauseMenu = null;

            if(null != _playerUI) {
                Destroy(_playerUI.gameObject);
            }
            _playerUI = null;
        }

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(null == _pauseMenu) {
                return;
            }

            if(PartyParrotManager.Instance.IsPaused) {
                _pauseMenu.gameObject.SetActive(true);
                _pauseMenu.Reset();
            } else {
                _pauseMenu.gameObject.SetActive(false);
            }
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ssjAug2018.UIManager");
            debugMenuNode.RenderContentsAction = () => {
                if(null != PlayerUI) {
                    string text = "Toggle HUD";
                    if(GUILayout.Button(text, GUIUtils.GetLayoutButtonSize(text))) {
                        PlayerUI.ToggleHUD();
                    }
                }
            };
        }
    }
}
