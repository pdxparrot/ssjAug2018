using pdxpartyparrot.Game.State;
using pdxpartyparrot.ssjAug2018.Loading;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class GameStateManager : pdxpartyparrot.Game.State.GameStateManager
    {
        protected override void ShowLoadingScreen(bool show)
        {
            LoadingManager.Instance.ShowLoadingScreen(show);
        }

        protected  override void UpdateLoadingScreen(float percent, string text)
        {
            LoadingManager.Instance.UpdateLoadingScreen(percent, text);
        }
    }
}
