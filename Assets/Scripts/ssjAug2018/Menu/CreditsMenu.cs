using pdxpartyparrot.ssjAug2018.GameState;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class CreditsMenu : Game.Menu.Menu
    {
#region Event Handlers
        public void OnBack()
        {
            GameStateManager.Instance.PopSubState();
        }
#endregion
    }
}
