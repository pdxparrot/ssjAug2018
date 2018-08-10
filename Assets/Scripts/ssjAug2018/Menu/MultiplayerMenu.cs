using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.ssjAug2018.GameState;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class MultiplayerMenu : MenuPanel
    {
#region Event Handlers
        public void OnHost()
        {
            GameStateManager.Instance.StartHost();
        }

        public void OnJoin()
        {
            GameStateManager.Instance.StartJoin();
        }

        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
