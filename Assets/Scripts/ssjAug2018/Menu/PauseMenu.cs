using pdxpartyparrot.Game.Menu;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class PauseMenu : MenuPanel
    {
        [SerializeField]
        private SettingsMenu _settingsMenu;

#region Unity Lifecycle
        private void Awake()
        {
            _settingsMenu.gameObject.SetActive(false);
        }
#endregion

#region Event Handlers
        public void OnSettings()
        {
            Owner.PushPanel(_settingsMenu);
        }

        public void OnExit()
        {
            Application.Quit();
        }
#endregion
    }
}
