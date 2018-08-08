using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.ssjAug2018.GameState;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class CreditsMenu : MenuPanel
    {
#region Event Handlers
        public void OnBack()
        {
            GameStateManager.Instance.PopSubState();
        }
#endregion
    }
}
