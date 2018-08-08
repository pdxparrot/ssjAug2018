using pdxpartyparrot.Game.Menu;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class SettingsMenu : MenuPanel
    {
#region Event Handlers
        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
